﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace WateringFlowerpots.Controls
{
    public class StepperControl : Grid
    {
        public static readonly BindableProperty CountProperty = BindableProperty.Create
            (nameof(Count),
            typeof(int),
            typeof(StepperControl),
            0,
            BindingMode.TwoWay);

        public int Count
        {
            get
            {
                return (int)GetValue(CountProperty);
            }
            set
            {
                SetValue(CountProperty, value);
            }
        }

        private readonly Button minusButton;
        private readonly Label label;
        private readonly Button plusButton;

        public StepperControl()
        {
            ColumnDefinitions = new ColumnDefinitionCollection()
            {
                new ColumnDefinition(){ Width = new GridLength(20, GridUnitType.Star)},
                new ColumnDefinition(){ Width = new GridLength(80, GridUnitType.Star)},
                new ColumnDefinition(){ Width = new GridLength(20, GridUnitType.Star)}
            };

            minusButton = new Button
            {
                BackgroundColor = Color.Transparent,
                ImageSource = ImageSource.FromResource("WateringFlowerpots.Resources.minus.png", typeof(StepperControl).GetTypeInfo().Assembly)
            };
            minusButton.Clicked += MinusButtonClicked;

            label = new Label
            {
                Text = "0",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.White
            };

            plusButton = new Button
            {
                BackgroundColor = Color.Transparent,
                ImageSource = ImageSource.FromResource("WateringFlowerpots.Resources.plus.png", typeof(StepperControl).GetTypeInfo().Assembly)
            };
            plusButton.Clicked += PlusButtonClicked;

            Children.Add(label, 0, 0);
            Grid.SetColumnSpan(label, 3);
            Children.Add(minusButton, 0, 0);
            Children.Add(plusButton, 2, 0);
        }

        private void PlusButtonClicked(object sender, EventArgs e)
        {
            Count++;
        }

        private void MinusButtonClicked(object sender, EventArgs e)
        {
            if (Count > 0)
            {
                Count--;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == CountProperty.PropertyName)
            {
                label.Text = Count.ToString();
            }
        }
    }
}

