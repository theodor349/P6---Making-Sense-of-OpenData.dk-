﻿using LabelRecognizer.Models;
using Shared.Models;
using Shared.Models.ObjectAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelRecognizer.Helpers
{
    public interface ITypeLabeler
    {
        Task AssignTypes(DatasetObject dataset);
    }

    public class TypeLabeler : ITypeLabeler
    {
        public Task AssignTypes(DatasetObject dataset)
        {
            var typeCounter = new TypeCounter("");
            IncrementTypes(dataset, typeCounter);
            SetTypes(dataset, typeCounter);

            return Task.CompletedTask;
        }

        private void SetTypes(DatasetObject dataset, TypeCounter typeCounter)
        {
            CheckStringDatas(dataset, typeCounter);

            foreach (var intermediateObject in dataset.Objects)
            {
                foreach (var attr in intermediateObject.Attributes)
                {
                    var counter = typeCounter.Get(attr.Name);
                    SetType(attr, counter);
                }
            }
        }

        private void CheckStringDatas(DatasetObject dataset, TypeCounter typeCounter)
        {
            foreach (var intermediateObject in dataset.Objects)
            {
                foreach (var attr in intermediateObject.Attributes)
                {
                    var counter = typeCounter.Get(attr.Name);
                    CheckStringType(attr, counter);
                }
            }
        }

        private void CheckStringType(ObjectAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.CheckStringParse(attribute);
            CheckStringTypeChildren(attribute, typeCounter);
        }


        private void CheckStringTypeChildren(ObjectAttribute attribute, TypeCounter typeCounter)
        {
            if (attribute.GetType() == typeof(ListAttribute))
            {
                var list = (List<ObjectAttribute>)attribute.Value;
                for (int i = 0; i < list.Count; i++)
                {
                    CheckStringType(list[i], typeCounter.Get(list[i].Name));
                }
            }
        }

        private void SetType(ObjectAttribute attribute, TypeCounter typeCounter)
        {
            AddLabels(attribute, typeCounter);
            AddLabelsToChildren(attribute, typeCounter);
        }

        private void AddLabelsToChildren(ObjectAttribute attribute, TypeCounter typeCounter)
        {
            if (attribute.GetType() == typeof(ListAttribute))
            {
                var list = (List<ObjectAttribute>)attribute.Value;
                for (int i = 0; i < list.Count; i++)
                {
                    SetType(list[i], typeCounter.Get(list[i].Name));
                }
            }
        }

        private static void AddLabels(ObjectAttribute attribute, TypeCounter typeCounter)
        {
            var totalLabelCount = typeCounter.Counter.Sum(x => x.Value);
            if (typeCounter.ContainsOnlyDoubleAndLong())
                attribute.AddLabel(PredefinedLabels.Double, 1);
            else if (typeCounter.ContainsNullAndOtherType())
                attribute.AddLabel(typeCounter.GetNotNullType(), 1);
            else if (typeCounter.CanParseTextAsOtherType())
                attribute.AddLabel(typeCounter.GetNotTextType(), 1);
            else if (typeCounter.ContainsTextAndOtherPrimitiveType())
                attribute.AddLabel(PredefinedLabels.Text, 1);
            else
            {
                foreach (var label in typeCounter.Counter)
                {
                    attribute.AddLabel(label.Key, ((float)label.Value) / totalLabelCount);
                }
            }
        }

        private void IncrementTypes(DatasetObject dataset, TypeCounter typeCounter)
        {
            foreach (var intermediateObject in dataset.Objects)
            {
                foreach (var attr in intermediateObject.Attributes)
                {
                    var counter = typeCounter.Get(attr.Name);
                    Increment(attr, counter);
                }
            }
        }

        private void Increment(ObjectAttribute attribute, TypeCounter typeCounter)
        {
            switch (attribute)
            {
                case NullAttribute a:
                    IncrementNull(a, typeCounter);
                    break;
                case ListAttribute a:
                    IncrementList(a, typeCounter);
                    break;
                case DateAttribute a:
                    IncrementDate(a, typeCounter);
                    break;
                case DoubleAttribute a:
                    IncrementDouble(a, typeCounter);
                    break;
                case LongAttribute a:
                    IncrementLong(a, typeCounter);
                    break;
                case TextAttribute a:
                    IncrementText(a, typeCounter);
                    break;
                case BoolAttribute a:
                    IncrementBool(a, typeCounter);
                    break;

                default:
                    throw new NotImplementedException("Type not handled: " + attribute.GetType());
            }
        }

        private void IncrementBool(BoolAttribute a, TypeCounter typeCounter)
        {
            typeCounter.Increment(PredefinedLabels.Bool);
        }

        private void IncrementList(ListAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.Increment(PredefinedLabels.List);
            var list = (List<ObjectAttribute>)attribute.Value;
            foreach (var attr in list)
            {
                var newTypeCounter = typeCounter.Get(attr.Name);
                Increment(attr, newTypeCounter);
            }
        }

        private void IncrementNull(NullAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.Increment(PredefinedLabels.Null);
        }

        private void IncrementDate(DateAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.Increment(PredefinedLabels.Date);
        }

        private void IncrementDouble(DoubleAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.Increment(PredefinedLabels.Double);
        }

        private void IncrementLong(LongAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.Increment(PredefinedLabels.Long);
        }

        private void IncrementText(TextAttribute attribute, TypeCounter typeCounter)
        {
            typeCounter.Increment(PredefinedLabels.Text);
        }
    }
}