   M S B A M L   `   `   `  ����L  HPermissionGranter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:PermissionGranter.ViewPermissionGranter.View  5   
   I)clr-namespace:PermissionGranter.ViewModelPermissionGranter.ViewModel  5   	   (    "PermissionGranter.View.GroupDetail   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:PermissionGranter.View     5   
   ?	viewModel)clr-namespace:PermissionGranter.ViewModel     5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	     � Title$  GroupDetail��5	   	   $	��480��6   $	��600��6*    � DataContext 5
   	   ��    MainViewModelLocator �� Source# [   �� Path$ GroupDetails�.  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Closing��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *WindowClosingCommand+   CommandParameter 6M   �� 5      .��5      � 	 � ColumnDefinitions	 5   
   �� 5      $��1* �6   �� 5      $��2* �6   .+�5   
   �� -   5        $��
spUserInfo��5      $��0��6'   $��5q�67   .+�5      �� 
 �� Orientation$
 
Horizontal=�6   .+�5      �� 
�������.��6/   Groepdetails6=   5      �� 5      $��5,0,0,0q�6   .��6%   
Groepnaam:61   �� 5      -   5        $��txtGroepnaam��5       �� 	MaxLength$ 50��6�     TextBoxPropertyStyle#��[ 6�   ��6*   ��  �� ElementName$ 	lstGroups��$ SelectedItem.GroupName� �� Mode$ TwoWay=�{�6{   �� $ 	lstGroups��$ SelectedItems.Count�6   �� 5      $��5,0,0,0q�6   .��6%   Omschrijving:64   �� 5      -   5        $��txtOmschrijving��5      $	 250��6�   #��[ 6�   ��6-   �� $ 	lstGroups��$ SelectedItem.Description�$ TwoWay=�{�6�   �� $ 	lstGroups��$ SelectedItems.Count�6  �� 5!      $��5,0,0,0q�6   .��6%   
Groepen:6/   �� 5"      $
��Auto=�6   .��5#      �� -   5        $��	lstGroups��5#      $��90��6r     ValidationStyle#��[ 5$      ��5#   +   �� *
AllItems+ �� SelectedItem 6L   �� *SelectedItem+��5%      �� 5&      .R�5'      �� ��6$   �� *	GroupName+5(      5*      5,      ]� 5.      $��0,2,2,0q�6   ��6*   �� $ 
spUserInfo��$ ActualWidth-2�.+�5/      �� -   5        $��btnAddGroup��5/      $
��Left=�6O   ��6,   �� *NewItemCommand+ +� FocusedElement 50      �� $ txtGroepnaam��.��6Z   New6_   �� 51      -   5        $��btnRemoveGroup��51        ButtonDeleteStyle#��[ 60   $��Right=�6[   ��6w   �� *RemoveItemCommand+{�6�   �� $ 	lstGroups��$ SelectedItems.Count�.��6�   Delete6�   53      �� 55      $��5,0,0,0q�6   .��6%   Gebruikers:62   �� 56      $
��Auto=�6   .��57      �� -   5        $��lstUsers��57      $��90��6s   ��6*   �� *
GroupUsers+ 6M   �� *SelectedUser+6s   58      �� 5:      -   5        $��cmbUsers��5:      $��0,5,0,2q�6r   ��6'   �� *
AllUsers+ 6I   �� *SelectedAddUser+6�   ]� 5<      $��3q�6   ��6$   �� $ 
spUserInfo��$ ActualWidth-2�.+�5=      �� -	   5        $��btnAddUserToGroup��5=      $
��Left=�62   ��6M   �� *AddUserToGroupCommand+{�6w   �� $ lstAllGroups��$ SelectedItems.Count�.��6�   Add6�   �� 5>      -
   5        $��btnRemoveUserFromGroup��5>      $
��Left=�67   #��[ 6�   ��6R   �� *RemoveUserFromGroupCommand+{�6�   �� $ 	lstGroups��$ SelectedItems.Count�.��6�   Remove6�   �� 5@      -   5        $��btnSave��5@      $
��Left=�6)     ButtonStyle#��[ 6d   ��6D   �� *SaveCommand+.��6�   Save6�   �� 5B      -   5        $��	btnCancel��5B      $��Right=�6+   ��6G   �� *CancelCommand+.��6i   Cancel6q   5C      5D      �� 5E   
   -   5        $��spCopy��5E   .   $
 
Horizontal=�6   $��1��6<    K� ZIndex$ 1��6L   $��Bottom=�6]   $
��Left=�6x   .+�5F      �� $
��Copy��6   #��[ 6D   ��6$   �� *CopyCommand+6D   �� 5G      $��Paste��6   #��[ 6F   ��6%   �� *PasteCommand+6F   5H      �� 5I   
   -   5        $��Permissions��5I      $��1��6*   $ 0��6:   .��5K      R� -   5        $��trvUser��5K      $��0,0,0,20q�6*   ��6=   �� *CompleteMenu+ � 	Resources 5L       �� DataType/   )PermissionGranter.ViewModel.CustTreeItems �� ItemsSource d� IsThreeState  SimpleCheckBox d� 	IsChecked  TreeViewFont �� Property  TopLevelTreeViewItemColor �� Value �� Triggers ^� 
Conditions �� Binding �� Value �� Binding �� Value  IsGreaterThanConverter �� 	Converter �� ConverterParameter  	 BottomLevelTreeViewItemColor,   &PermissionGranter.ViewModel.Permission  �� Padding! J� SnapsToDevicePixels 
 CheckBoxTemplate
" � Tag# � ToolTip  ColoredToolTip  ToolTipForeGround��%&  (��       5M   3   *, +)7  7  7  7  �� 6   "  63    6_   �� *Items+.R�5N      �� $
 
Horizontal=�6)   .+�5O      ��  . $��1,1,2,1q�6;   8��   6L   �� *	HasAccess+$ TwoWay=�6y   �� 5P   "   8�� ��6,   �� $
 Name���5Q   &   �� 5R   *   �5S   .   �� 5T   2    � ��# �  6Y    5V   .   ^� 5W   2    5X   6   �� 5Y   :    6D   �� *	HasParent+ True6b   �� 5Z   :    6D   �� *Options.Count+ 06f   .0�5\   6   ��  � ��$ 18(�6[   6h   5]   3   �� 5^   2    6>   �� *	HasParent+	 false6\   .a�5_   6   ��  � ��$
 Bold"�6]   �� 5`   6    � ��$ 20(�6[   5a   3   �� 5f   2    6>   �� *Options.Count+8  $ 0�� true6�   .a�5g   6   ��  � ��$ 15(�6[   �� 5h   6    � ��# � 	 6]   5i   3   5k   +   5m   #   �� 5n   "   $��2,0,0,0q�6*   
������ $��0q�6T   ��6h   �� *	Options+��5o   &   �� 5p   *   .R�5q   .   �� $
 
Horizontal=�69   5r   +   ��5t   &   �� 5u   *   "  68   (��       *, +).R�5v   .   �� $��Border��65   $  0q�6C   ! . 
������ $��0q�6�   ._�5w   2   �� 8��  . $��0q�6�   $��0q�6�   #��� 
 6�   " 6w   �� *Name+ 6�   �� *Value+$ TwoWay=�# 5x   6   _� 5y   :   #���  6B   .��5{   >   �� 
������ .+�5|   B   �� #���  6L   $
��Bold"�6�   
������ ��6}   �� *Name+5~   C   �� 5   B   #���  6L   
������ ��6}   �� *Description+6�   5�   ?   5�   ;   5�   3   5�   /   5�   +   5�   #   5�      5�      5�      5�      5�      5�      5�      