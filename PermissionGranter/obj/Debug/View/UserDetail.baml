   M S B A M L   `   `   `  ����L  HPermissionGranter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:PermissionGranter.ViewPermissionGranter.View  5   	   I)clr-namespace:PermissionGranter.ViewModelPermissionGranter.ViewModel  5   	   '    !PermissionGranter.View.UserDetail   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:PermissionGranter.View     5   	   ?	viewModel)clr-namespace:PermissionGranter.ViewModel     5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	     � Title$  
UserDetail��5	   	   $	��570��6   $	��600��6)    � DataContext 5
   	   ��    MainViewModelLocator �� Source# [   �� Path$ UserDetails�.  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Closing��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *WindowClosingCommand+   CommandParameter 6M   �� 5      .��5      � 	 � ColumnDefinitions	 5   
   �� 5      $��1* �6   �� 5      $��2* �6   .+�5   
   �� -   5        $��
spUserInfo��5      $��0��6'   $��5q�67   .+�5      �� 
 �� Orientation$
 
Horizontal=�6   .+�5      �� 
�������.��6/   Userdetails6<   5      �� 5      $��5,0,0,0q�6   .��6%   	Voornaam:60   �� 5      -   5        $��txtVoornaam��5       �� 	MaxLength$ 30��6^     TextBoxPropertyStyle#��[ 6m   ��6)   �� *SelectedItem.FirstName+ �� Mode$ TwoWay=�6�   �� 5      $��5,0,0,0q�6   .��6%   Achternaam:62   �� 5      -   5        $��txtAchternaam��5      $ 30��6_   #��[ 6n   ��6+   �� *SelectedItem.LastName+$ TwoWay=�6�   �� 5       $��5,0,0,0q�6   .��6%   Wachtwoord:62   �� 5!      -   5        $��txtWachtwoord��5!      $ 50��6_   #��[ 6n   ��6+   �� *SelectedItem.Password+$ TwoWay=�6�   �� 5#      $��5,0,0,0q�6   .��6%   	E-mail:6.   �� 5$      -   5        $��txtEmail��5$      $ 80��6W   #��[ 6f   ��6&   �� *SelectedItem.Email+$ TwoWay=�6�   �� 5&      $��5,0,0,0q�6   .��6%   
Groepen:6/   �� 5'      $
��Auto=�6   .��5(      �� -   5        $��	lstGroups��5(      $��90��6u   ��6+   �� *
UserGroups+ �� SelectedItem 6N   �� *SelectedGroup+6u   5)      �� 5*      -   5        $��lstAllGroups��5*      ��6+   �� *	AllGroups+ 6M   �� *ToAdd+5,      ]� 5-      $��3q�6   .+�5.      �� -   5        $��btnAddGroup��5.      $
��Left=�6,   ��6G   �� *AddUserToGroupCommand+{�6q   ��  �� ElementName$ lstAllGroups��$ SelectedItems.Count�.��6�   Add6�   �� 5/      -	   5        $��btnRemoveGroup��5/      $��Right=�6/   $��3,0,0,0q�6z   ��6K   �� *RemoveUserFromGroupCommand+{�6�   �� $ lstAllUsers��$ SelectedItems.Count�.��6�   Remove6�   51      �� 52      $��5,0,0,0q�6   .��6%   Gebruikers:62   �� 53      $
��Auto=�6   .��54      �� -
   5        $��lstAllUsers��54      $��90��6u     ValidationStyle#��[ 55      ��54   -   �� *
AllItems+ 6N   �� *SelectedItem+��56      �� 57      .R�58      �� ��6$   �� *Name+59      5;      5<      ]� 5?      $��3,5,3,3q�6   ��6*   �� $ 
spUserInfo��$ ActualWidth-2�.+�5@      �� -   5        $��btnSave��5@      $
��Left=�6(   $��0,0,0,0q�6C   ��6T   �� *SaveCommand+.��6t   Save6z   �� 5A      -   5        $��btnNew��5A      $
��Left=�6'   $��3,0,0,0q�6B   $	��New��6v   ��6S   �� *NewItemCommand+ +� FocusedElement 5B      �� $ txtVoornaam���� 5C      -   5        $��	btnDelete��5C      $
��Left=�6*     ButtonDeleteStyle#��[ 6k   ��6E   �� *RemoveItemCommand+{�6�   �� $ lstAllUsers��$ SelectedItems.Count�.��6�   Delete6�   �� 5E      -   5        $��	btnCancel��5E      $��Right=�6*   $��3,0,0,0q�6G   ��6X   �� *CancelCommand+.��6z   Cancel6�   5G      5H      �� 5I   
   -   5        $��spCopy��5I   .   $
 
Horizontal=�6   $��1��6<    K� ZIndex$ 1��6L   $��Bottom=�6]   $
��Left=�6x   .+�5J      �� $
��Copy��6     ButtonStyle#��[ 6D   ��6$   �� *CopyCommand+6D   �� 5K      $��Paste��6   #��[ 6F   ��6%   �� *PasteCommand+6F   5L      �� 5M   
   -   5        $��Permissions��5M      $��1��6*   $ 0��6:   .��5N      R� -   5        $��trvUser��5N      $��0,0,0,20q�6&   $��0��69   $��1��6I   ��6V   �� *CompleteMenu+ � 	Resources 5O       �� DataType/   )PermissionGranter.ViewModel.CustTreeItems �� ItemsSource d� IsThreeState  SimpleCheckBox d� 	IsChecked  TreeViewFont �� Property  TopLevelTreeViewItemColor �� Value �� Triggers ^� 
Conditions �� Binding �� Value �� Binding �� Value  IsGreaterThanConverter �� 	Converter �� ConverterParameter  	 BottomLevelTreeViewItemColor,   &PermissionGranter.ViewModel.Permission  �� Padding! J� SnapsToDevicePixels 
 CheckBoxTemplate
" � Tag# � ToolTip  ColoredToolTip  ToolTipForeGround��%&  (��       5P   /   *, +)7  7  7  7  �� 6   "  6/    6[   �� *Items+.R�5Q      �� $
 
Horizontal=�6%   .+�5R      ��  . $��1,1,2,1q�6;   8��   6L   �� *	HasAccess+$ TwoWay=�6y   �� 5S      8�� ��6(   �� $
 Name���5T   "   �� 5U   &   �5V   *   �� 5W   .    � ��# �  6U    5Y   *   ^� 5Z   .    5[   2   �� 5\   6    6@   �� *	HasParent+ True6^   �� 5]   6    6@   �� *Options.Count+ 06b   .0�5_   2   ��  � ��$ 18(�6W   6d   5`   /   �� 5a   .    6:   �� *	HasParent+	 false6X   .a�5b   2   ��  � ��$
 Bold"�6Y   �� 5c   2    � ��$ 20(�6W   5d   /   �� 5i   .    6:   �� *Options.Count+8  $ 0�� true6�   .a�5j   2   ��  � ��$ 15(�6W   �� 5k   2    � ��# � 	 6Y   5l   /   5n   '   5p      �� 5q      $��2,0,0,0q�6&   
������ $��0q�6P   ��6d   �� *	Options+��5r   "   �� 5s   &   .R�5t   *   �� $
 
Horizontal=�65   5u   '   ��5w   "   �� 5x   &   "  64   (��       *, +).R�5y   *   �� $��Border��61   $  0q�6?   ! . 
������ $��0q�6   ._�5z   .   �� 8��  . $��0q�6�   $��0q�6�   #��� 
 6�   " 6s   �� *Name+ 6�   �� *Value+$ TwoWay=�# 5{   2   _� 5|   6   #���  6>   .��5~   :   �� 
������ .+�5   >   �� #���  6H   $
��Bold"�6�   
������ ��6y   �� *Name+5�   ?   �� 5�   >   #���  6H   
������ ��6y   �� *Description+6�   5�   ;   5�   7   5�   /   5�   +   5�   '   5�      5�      5�      5�      5�      5�      5�      5�      