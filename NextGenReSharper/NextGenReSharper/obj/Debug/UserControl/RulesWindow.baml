   M S B A M L   `   `   `  ����H  DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5      K GNextGenReSharper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:NextGenReSharper.UserControlNextGenReSharper.UserControl 5   	   .   (NextGenReSharper.UserControl.RulesWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Pcontrols;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5      @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <local*clr-namespace:NextGenReSharper.UserControl     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5	   	   (   "MahApps.Metro.Controls.MetroWindow   WindowTitleBrush �	#FFB35546 � Title$ Rules��5   	   $	��400��6   $	��600��6$    � 
ResizeMode$ NoResize=�6L    � 	Resources 5         NgButton �� 
TargetType  MetroFlatButton �� BasedOn �� Property �� Value��%�   &	        7  �� 5   
   " ��5      8   .�5      ��  � ��
 �����6+   �� 5       � �� �	#FFB355466+   5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   
 � ColumnDefinitions
 5!   
   �� 5"      �� 5#      .+�5%   
   �� $��Layers settings��6   $��Center=�6+   �� 5&   
   $��1��6   $��Create Engine (BL)��5'       d� 	IsChecked$
 TrueY�5(      {�.  $��	21,0,49,0q�5*      �� 5+   
   -   5        $��chkDAL��5,      $��2��5+      $��Create Data Access (DAL)��5.      $��21,0,0,0q�5/      �� 50   
   -   5        $��chkModel��51      $��3��50      $��Create Models��52      $��21,0,0,0q�53      �� 55   
   -   5        $��chkContract��56      $��4��55      $"��Create Contracts (Interface)��57      $��21,0,0,0q�58      �� 5:   
   $��5��6   $��Unittest settings��5;      $��Center=�5<      �� 5>   
   -   5        $��chkAUTforBL��5?      $��6��5>      $��Create AUT for Engine��5@      $��21,0,0,0q�5A      �� 5C   
   -   5        $��chkAUTforDAL��5D      $��7��5C      $ ��Create AUT for Data Access��5E      $��21,0,0,0q�5F      �� 5H   
   -   5        $��chkAddNameSpace��5J      $��3��5H      $��1��5I      $��Add Namespace��5K      $��21,0,0,0q�5L      �� 5O   
   -   5        $��chkCreateProject��5Q      $��2��5O      $��1��5P      $��Create C# .Net Project��5R      $��21,0,0,0q�5S      �� 5T   
   $��0��6   $��1��6!   $��Other setting��61   $��Center=�6F   �� 5V   
   -	   5        $��chkSPComment��5X      $��1��5V      $��1��5W      $��Convert SP Comment��5Y      $��21,0,0,0q�5Z      �� 5[   
   -
   5         ��Name$ btnRules��5\      #��[  5[      $��1��5]      $��Save and Close��5^      $��167,111,10,-112q�5`      $��7��5a      5e      5f      