   M S B A M L   `   `   `  ����L  HPermissionGranter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:PermissionGranter.ViewPermissionGranter.View  5   	   I)clr-namespace:PermissionGranter.ViewModelPermissionGranter.ViewModel  5   	   "    PermissionGranter.View.Login   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6local$clr-namespace:PermissionGranter.View     5   	   ?	viewModel)clr-namespace:PermissionGranter.ViewModel     5   	   -   5          � Title$  Login��5   	   $	��300��6   $	��300��6$    � DataContext 5   	   ��    MainViewModelLocator �� Source# [   �� Path$ UserSession�.��5      �� $��
50,10,50,0q�6   .+�5	   
   ��  �� TextAlignment$ Center=�62   $��0,0,0,20q�6I   
���  ����6   �� *ErrorMessage+6n   �� 5
   
   $��Login��6   $ Center=�6!   $��0,0,0,20q�68   6L   �� 5   
   -   5        $��TxtEmail��5      $��e-mail��6$   $ Center=�62   6K   �� 5   
   -   5        $��Email��5       �� 	MaxLength$ 80��66   ��6   �� *Email+6G   �� 5   
   $��Password��6   $ Center=�6$   6=   �� 5   
   -   5        $��TxtPassword��5      $ 80��6?   ��6%   �� *
Password+6P   �� 5   
    �� Orientation$ 
Horizontal=�6   $��Center=�6.   $��10q�6K   .+�5      �� -   5        $��BtnLogin��5      $��65��6&   ��61   �� *LoginCommand+.��6R   Login6Y   �� 5      -   5        $��	btnCancel��5      $��Cancel��6&   $��65��6Y   ��67   �� *CancelCommand+6Y   5      �� 5   
   -   5        $��btnCreateUsers��5      $��
Edit Users��6'   $��65��6<   ��6G   �� *
UserDetail+6h   �� 5   
   -   5        $��btnCreateGroups��5      $��0,5,0,0q�6(   $��Edit Groups��69   $��65��6O   ��6Z   �� *GroupDetail+6|   5      5      