﻿namespace Shared.Models.ObjectAttributes
{
    public class LabelModel
    {
        public string Label { get; set; }
        public float Probability { get; set; }

        public LabelModel(string label, float probability)
        {
            Label = label;
            Probability = probability;
        }

        public LabelModel(string label)
        {
            Label = label;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            LabelModel? label = obj as LabelModel;

            if (label != null)
                return Equals(label);
            else return false;
        }
        private bool Equals(LabelModel label)
        {
            return Label == label.Label;
        }

        public override int GetHashCode()
        {
            return Label.GetHashCode();
        }
    }
    
    public abstract class ObjectAttribute
    {
        public string Name { get; }
        public object Value { get; }
        public List<LabelModel> Labels { get; set; } = new List<LabelModel>();

        public ObjectAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public void AddLabel(string label, float probability)
        {
            Labels.Add(new LabelModel(label, probability));
        }

        public bool HasLabel(string label)
        {
           return GetLabel(label) != null;
        }
        public LabelModel? GetLabel(string label)
        {
            return Labels.FirstOrDefault(x => x.Label == label);
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class LongAttribute : ObjectAttribute
    {
        public LongAttribute(string name, long value) : base(name, value)
        {
        }
    }

    public class DoubleAttribute : ObjectAttribute
    {
        public DoubleAttribute(string name, double value) : base(name, value)
        {
        }
    }

    public class DateAttribute : ObjectAttribute
    {
        public DateAttribute(string name, DateTime value) : base(name, value)
        {
        }

        public override string ToString()
        {
            var d = (DateTime)Value;
            return d.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }

    public class TextAttribute : ObjectAttribute
    {
        public TextAttribute(string name, string value) : base(name, value)
        {
        }
    }
    public class NullAttribute : ObjectAttribute
    {
        public NullAttribute(string name) : base(name, "null")
        {
        }
    }

    public class ListAttribute : ObjectAttribute
    {
        public ListAttribute(string name) : base(name, new List<ObjectAttribute>())
        {
        }

        public ListAttribute(string name, List<ObjectAttribute> list) : base(name, list)
        {  
        }
    }

    public class BoolAttribute : ObjectAttribute
    {
        public BoolAttribute(string name, bool value) : base(name, value)
        {
        }
    }
}
