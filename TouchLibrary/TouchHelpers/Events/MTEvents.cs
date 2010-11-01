/*
TouchFramework connects touch tracking from a tracking engine to WPF controls 
allow scaling, rotation, movement and other multi-touch behaviours.

Copyright 2009 - Mindstorm Limited (reg. 05071596)

Author - Simon Lerpiniere

This file is part of TouchFramework.

TouchFramework is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

TouchFramework is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser Public License for more details.

You should have received a copy of the GNU Lesser Public License
along with TouchFramework.  If not, see <http://www.gnu.org/licenses/>.

If you have any questions regarding this library, or would like to purchase 
a commercial licence, please contact Mindstorm via www.mindstorm.com.
*/

using System;
using System.Windows;
using System.Windows.Markup;

namespace TouchFramework.Events
{
    /// <summary>
    /// UI routed events to be raised in UIElements when touch actions occur.
    /// </summary>
    public static class MTEvents
    {
        public static readonly RoutedEvent TapEvent = EventManager.RegisterRoutedEvent(
                "Tap", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FrameworkElement));
        public static readonly RoutedEvent SlideEvent = EventManager.RegisterRoutedEvent(
                "Slide", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FrameworkElement));
        public static readonly RoutedEvent ScrollEvent = EventManager.RegisterRoutedEvent(
                "Scroll", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FrameworkElement));
        public static readonly RoutedEvent TouchDownEvent = EventManager.RegisterRoutedEvent(
                "TouchDown", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FrameworkElement));
        public static readonly RoutedEvent TouchUpEvent = EventManager.RegisterRoutedEvent(
                "TouchUp", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FrameworkElement));
        public static readonly RoutedEvent DragEvent = EventManager.RegisterRoutedEvent(
                "Drag", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FrameworkElement));
    }
}
