   M S B A M L   `   `   `  ����J  FVR-registration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:VR_registrationVR_registration  5           VR_registration.MainWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:VR_registration     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      -   5          �Name$  MAIN_WINDOW��5       � Title$ Главное меню��5	      $
��1100��5
      $	��900��5       � AllowsTransparency .   � 
ResizeMode$ NoResize=�5       � WindowStyle$
 None=�5      .��5      � .+�5   
   � -   5         �Name$
 Head��5         
head_style#��[  5      .+�5      �� -   5        $��Picture/cross.png��5        cross_style#��[ 5      � 5      -   5        $ Org_Information��5        OrgInf_style#��[ 6,   .+�5      ��   OrgName_style#��[ 6   $��VR клуб��6C   �� 5      -   5         ��Name$ 
buttonGoVk��5      $��0,70,0,0q�5        ChatsIconsBorder_style#��[ 5      ._�5       � .+�5!      �� $��Picture/vk.png��6      ChatsIcons_style#��[ 68   �� 5"      -   5          button_style#��[ 5"   .   5#      5%      �� 5&      -   5        $ buttonGoTelegram��5'      $��
0,70,100,0q�5(      #��[ 5)      ._�5*      � .+�5+      �� $��Picture/tg.png��6    #��[ 68   �� 5,      -   5        #��[ 5,   4   5-      5/      �� 50      -	   5        $ buttonGoYouTube��51      $��
100,70,0,0q�52      #��[ 53      ._�54      � .+�55      �� $��Picture/youtube.png��6    #��[ 6=   �� 56      -
   5        #��[ 56   3   57      59      5<      � 5>        buttons_block#��[ 6   .+�5@      �� -   5        $ shadow_UserButton��5A      $��Center=�5B        shadow_head_btn#��[ 5C      �� 5D      -   5        $ shadow_goBackButton��5E      $��Right=�5F      #��[ 5G      �� 5K      -   5        $ 
UserButton��5L      $��Center=�5M       	 head_btn#��[	 5N      ._�5O      � .+�5P      �� $��Picture/user.png��6     
 icons_style#��[
 6:   �� 5Q      -   5         ��Name$ go_user_acc��5R      #��[ 5T      5V      5Z      �� 5]      -   5        $ goBackButton��5^      $��Right=�5_      #��[	 5`      ._�5a      � .+�5c      �� $
��Left=�5d       J� RenderTransformOrigin$ 0.52,0&�5e      $��Picture/back.png��5f      #��[
 5g      z�5h      �� 5i   "   	 �� ScaleX$	 -1Q�61   5l      �� 5m      -   5        $ go_back��5n      #��[ 5p      5r      5u      5w      5x      � 5{   
   -   5        $
 Body��5|      
 � ShowGridLines
 .    
body_style#��[ 5~       � ColumnDefinitions 5      �� 5�      $	��700 �6#   �� 5�      .+�5�      �� $��1��5�      $	��650��5�      $��Center=�5�      $��Picture/Sale.jpeg��5�      � 5�      $��0��6   .+�5�      ��   bookPlace_btn_style#��[ 6   ._�5�      � .+�5�      ��   normalText_style#��[ 6$   $ ��Забронировать��6N   �� 5�      -   5        $ 
book_place��5�      #��[ 5�      5�      5�      �� 5�      -   5        $ Poster��5�        poster_style#��[ 6)   ��5�      �� 5�      -   5        Posters�� j� Stretch$
 Fill=�5�   6   5�      5�      5�      5�      5�      