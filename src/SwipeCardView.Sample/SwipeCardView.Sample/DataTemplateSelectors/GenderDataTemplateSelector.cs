using System;
using System.Collections.Generic;
using System.Text;
using SwipeCardView.Sample.Model;
using Xamarin.Forms;

namespace SwipeCardView.Sample.DataTemplateSelectors
{
    public class GenderDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MaleDataTemplate { get; set; }

        public DataTemplate FemaleDataTemplate { get; set; }

        public DataTemplate DefautlDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Gender gender)
            {
                switch (gender)
                {
                    case Gender.Female:
                        return MaleDataTemplate;
                    case Gender.Male:
                        return FemaleDataTemplate;
                }
            }

            return DefautlDataTemplate;
        }
    }
}