   M S B A M L   `   `   `  ����H  DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	   K GNextGenReSharper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:NextGenReSharper.UserControlNextGenReSharper.UserControl 5   	   .   (NextGenReSharper.UserControl.ConfigWidow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Pcontrols;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <local*clr-namespace:NextGenReSharper.UserControl     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5	   	   (   "MahApps.Metro.Controls.MetroWindow   WindowTitleBrush �	#FFB35546 � Title$ Config��5   	   $	��400��6   $	��600��6%    � 
ResizeMode$ NoResize=�61    � 	Resources 5         NgButton �� 
TargetType  MetroFlatButton �� BasedOn �� Property �� Value��%�   &	        7  �� 5   
   " ��5      8   .�5      ��  � ��
 �����6+   �� 5       � �� �	#FFB355466+   5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5      $��30 �6   �� 5       $��30 �6   
 � ColumnDefinitions
 5"   
   �� 5#      �� 5$      �� 5%      .+�5'   
   �� $�� ��6   �� 5)   
   $��1��6   $��NameSpace Prefix : ��6"   $��Right=�6=   �� 5*   
   -   5        $��txtNamespaceName��5*      $��1��6*   $��1��68   $��0,0,0,5q�6H   �� 5,   
   $��2��6   $��Select Method Name :  ��6!   $��Right=�6?   �� 5-   
   -   5        $��txtSelectMethodName��5-      $��2��6-   $��1��6:   $��0,0,0,5q�6J   �� 5/   
   $��3��6   $��Insert Method Name :  ��6!   $��Right=�6?   �� 50   
   -   5        $��txtInsertMethodName��50      $��3��6-   $��1��6;   $��0,0,0,5q�6K   �� 52   
   $��4��6   $��Update Method Name :  ��6!   $��Right=�6?   �� 53   
   -   5        $��txtUpdateMethodName��53      $��4��6-   $��1��6:   $��0,0,0,10q�6J   �� 55   
   $��5��6   $��Delete Method Name :  ��6!   $��Right=�6?   �� 56   
   -   5        $��txtDeleteMethodName��56      $��5��6-   $��1��6:   $��26��6J   $	��Top=�6V   �� 58   
   $��6��6   $��Execute Method Name :  ��6!   $��Right=�6@   �� 59   
   -   5        $��txtExecuteMethodName��59      $��6��6.   $��1��6;   $��0,0,0,10q�6K   �� 5:   
   -   5         ��Name$ btnRules��5;      #��[  5:      $��2��5<      $��Save and Close��5=      $��68,49,10,-50q�5?      $��9��5@      5A      5B      