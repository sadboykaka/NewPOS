   M S B A M L   `   `   `  ����c  _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity  5   	    Accounts_Pos8   2Accounts_Pos.View.SalesOrder.SalesOrderEnquiryForm   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ^bPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5   	   -   5          �Name$  SalesOrderEnquiryWindow��5   	    � Title$ SalesOrderEnquiryForm��5   	   $	��300��6'   $	��700��64   .��5      � .+�5   
   ��    Instrcuttextitalicprod#��[  6   $
��Left=�6P   $��	47,40,0,0q�6k   $
��Wrap=�6~   $	��Top=�6�   $	��167��6�   $��25��6�   �� 5	      -   5        .�5
   ,   Customer Code:5      5      �� 5   
   -   5        $��CustomerCodeTxt��5      $��2��6x   $
��Left=�6�   $��23��6�   $��
220,40,0,0q�6�    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��120��6�   ��6*   �� *CustomerCode+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5   
   -   5        $��CustomerNameTxt��5      $��2��6+   $
��Left=�6?   $��23��6Z   $��
350,40,0,0q�6f   $
 Wrap=�6z   $	��Top=�6�   $	��220��6�   �� 5   
   #��[  6   $
��Left=�6P   $��	47,80,0,0q�6k   $
��Wrap=�6~   $	��Top=�6�   $	��167��6�   $��25��6�   �� 5      -   5        .�5   )   Sales Order Number:5      5      �� 5   
   -   5        $��SalesOrderTxt��5      $��2��5      $
��Left=�5      $��23��5      $��
220,80,0,0q�5      $
 Wrap=�5      $	��Top=�5      $	��120��5      ��5      �� *	OrderNo+$ TwoWay=�$ PropertyChanged=�.��6[   .   (System.Windows.Interactivity.Interaction   Triggers 5       /   )System.Windows.Interactivity.EventTrigger  5!         	EventName$ TextChanged��6!   .   (System.Windows.Interactivity.TriggerBase   Actions. 5"      6   0System.Windows.Interactivity.InvokeCommandAction     Command 6,   �� *OrderNoChanged+5#      .��5$   &   5%      �� 5&   
   $��Print��6     
AddBtnLook#��[ 6B   $��
50,200,0,0q�6f   $
��Left=�6z   $	��Top=�6�   ��6!   �� *PrintCommand+6�   �� 5'   
   $��Include Address Details��6z   $
��Left=�6�   $��
387,80,0,0q�6�   $	��Top=�6�   	 d� 	IsChecked	 6   �� *IncludeAddressDetailsChecked+{�6G   �� *IncludeAddressDetailsEnabled+6�   5(      5)      