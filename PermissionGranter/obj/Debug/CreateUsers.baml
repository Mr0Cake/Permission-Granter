   M S B A M L   `   `   `  ����L  HPermissionGranter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:PermissionGranterPermissionGranter  5      I)clr-namespace:PermissionGranter.ViewModelPermissionGranter.ViewModel  5      _4clr-namespace:PermissionGranter.ViewModel.Converters&PermissionGranter.ViewModel.Converters  5      #    PermissionGranter.CreateUsers   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:PermissionGranter     5      ?	viewModel)clr-namespace:PermissionGranter.ViewModel     5      J	converter4clr-namespace:PermissionGranter.ViewModel.Converters     5      .��5      �   � ColumnDefinitions  5   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      $
��auto �6   $��10��6*   �� 5      �� 5      .+�5   
   �� $��0��6   $��1��6%   .+�5      �� $��	Firstname��6   �� 5      -   5         ��Name$ txtFirstName��5      �� 5      $��Lastname��6   �� 5      -   5        $ txtLasttName��5      �� 5      $��Function��6   �� 5      -   5        $ txtFunction��5      �� 5      -   5         ��Name$ 	btnCreate��5      $��Create��6@   5      o� 5   
   -   5        $	��mnu��5      $��0��6   $��0��6*   $��2��67   ��6K   �� *	MenuItems+��5      �� 5        �� 
TargetType" m�6   .�5!      ��  �� Property �@m�Command �� Value 60   �� *	Command+5"      ��5$      �� 5%       �� DataType/   )PermissionGranter.ViewModel.CustTreeItems"  6+    �� ItemsSource 6W   �� *Items+.R�5&      �� ��6    �� *Name+5'      5)      �� 5*   
   -   5        	 ��Name$	 	spButtons��5*      $��1��6(   $��1��68   �� 5+   
   $��0��6   $��2��6'   $
��Auto=�64   $	��150��6d   .��5,      R� -   5        $��trvUser��5,      ��6'   �� *	MenuItems+
 � 	Resources
 5-       �� Orientation d� IsThreeState d� 	IsChecked �� Mode   TreeViewFont �� Path  TopLevelTreeViewItemColor �� Triggers ^� 
Conditions �� Binding �� Value �� Binding �� Value  IsGreaterThanConverter �� 	Converter �� ConverterParameter   BottomLevelTreeViewItemColor,   &PermissionGranter.ViewModel.Permission �� Padding J� SnapsToDevicePixels  SimpleCheckBox  CheckBoxTemplate
 � Tag � ToolTip  ColoredToolTip  ToolTipForeGround��%  (��       5.   /   *, +)7   7  7  �� 6   "  6/    6[   �� *Items+.R�5/      �� $ 
Horizontal=�6%   .+�50      ��  . $��1,1,2,1q�6;    6L   �� *	HasAccess+$ TwoWay=��� 51      8��  ��6(   �� $
 Name���52   "   �� 53   &   �54   *   �� 55   .    � ��# �  6U    57   *   ^� 58   .    59   2   �� 5:   6    6@   �� *	HasParent+ True6^   �� 5;   6    6@   �� *Options.Count+ 06b   .0�5=   2   ��  � ��$ 18(�6W   6d   5>   /   �� 5?   .    6:   �� *	HasParent+	 false6X   .a�5@   2   ��  � ��$
 Bold"�6Y   �� 5A   2    � ��$ 20(�6W   5B   /   �� 5G   .    6:   �� *Options.Count+8  $ 0�� true6�   .a�5H   2   ��  � ��$ 15(�6W   �� 5I   2    � ��# �  6Y   5J   /   5L   '   5N      �� 5O      $��2,0,0,0q�6&   
������ $��0q�6P   ��6d   �� *	Options+��5P   "   �� 5Q   &   .R�5R   *   �� $ 
Horizontal=�65   5S   '   ��5U   "   �� 5V   &   "  64   (��       *, +).R�5W   *   �� $��Border��61   $ 0q�6?    . 
������ $��0q�6   ._�5X   .   �� 8��  . $��0q�6�   $��0q�6�   #���  6�    6s   �� *Name+ 6�   �� *Value+$ TwoWay=� 5Y   2   _� 5Z   6   #���  6>   .��5\   :   �� 
������ .+�5]   >   �� #���  6H   $
��Bold"�6�   
������ ��6y   �� *Name+5_   ?   �� 5`   >   #���  6H   
������ ��6y   �� *Description+6�   5a   ;   5b   7   5d   /   5e   +   5f   '   5h      5i      5j      5l      5m      5n      �� 5o   
   $��2��6   $��1��6"   .+�5p      �� -   5         ��Name$ lstUsers��5p      $
��Auto=�6V   �� 5q      -	   5        $��	txtOutput��5q      6+   5r      5s      5u      