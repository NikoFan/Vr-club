   M S B A M L   `   `   `  ����J  FVR-registration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:VR_registrationVR_registration  5      &     VR_registration.ClubBookingClass   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:VR_registration     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      -   5          �Name$  BOOKCLUB_WINDOW��5       � Title$ Бронь клуба��5	      $
��1100��5
      $	��900��5       � AllowsTransparency .   � 
ResizeMode$ NoResize=�5       � WindowStyle$
 None=�5      .��5      � .+�5   
   � -   5         �Name$
 Head��5         
head_style#��[  5      .+�5      �� -   5        $��Picture\cross.png��5        cross_style#��[ 5      � 5      -   5        $ Org_Information��5        OrgInf_style#��[ 6,   .+�5      ��   OrgName_style#��[ 6   $��VR клуб��6C   �� 5      -   5         ��Name$ 
buttonGoVk��5      $��0,70,0,0q�5        ChatsIconsBorder_style#��[ 5      ._�5       � .+�5!      �� $��Picture\vk.png��6      ChatsIcons_style#��[ 68   �� 5"      -   5          button_style#��[ 5"   .   5#      5%      �� 5&      -   5        $ buttonGoTelegram��5'      $��
0,70,100,0q�5(      #��[ 5)      ._�5*      � .+�5+      �� $��Picture\tg.png��6    #��[ 68   �� 5,      -   5        #��[ 5,   4   5-      5/      �� 50      -	   5        $ buttonGoYouTube��51      $��
100,70,0,0q�52      #��[ 53      ._�54      � .+�55      �� $��Picture\youtube.png��6    #��[ 6=   �� 56      -
   5        #��[ 56   3   57      59      5<      � 5>        buttons_block#��[ 6   .+�5@      �� -   5        $ shadow_UserButton��5A      $��Center=�5B        shadow_head_btn#��[ 5C      �� 5D      -   5        $ shadow_goBackButton��5E      $��Right=�5F      #��[ 5G      �� 5I      -   5        $ 
UserButton��5J      $��Center=�5K       	 head_btn#��[	 5L      ._�5M      � .+�5N      �� $��Picture\user.png��6     
 icons_style#��[
 6:   �� 5O      -   5         ��Name$ go_user_acc��5P      #��[ 5R      5T      5U      �� 5W      -   5        $ goBackButton��5X      $��Right=�5Y      #��[	 5Z      ._�5[      � .+�5\      �� $
��Left=�5]       J� RenderTransformOrigin$ 0.52,0&�5^      $��Picture\back.png��5_      #��[
 5`      z�5a      �� 5b   "   	 �� ScaleX$	 -1Q�61   5e      �� 5f      -   5        $ go_back��5g      #��[ 5i      5k      5n      5p      5q      � 5t   
   -   5        $
 Body��5u      
 � ShowGridLines
 .    
body_style#��[ 5w      .+�5x      ��  J� Opacity$	 0.2Q�6   $)��#Picture\clubInformationWindowBG.png��6"   �� 5z       $  clubInformationShowerPlace_style#��[ 6   ._�5{      �� .+�5|      ��   captionTextBlock_style#��[ 6    $��
Адрес��6P   �� 5}      -   5         ��Name$ clubAddressTextBox��5~         clubInformationTextBox_style#��[ 5      $��Address��5�      �� 5�      #��[ 6    $��
Почта��6P   �� 5�      -   5        $ clubMailTextBox��5�      #��[ 5�      $
��Mail��5�      �� 5�      #��[ 6    $��Телефон��6P   �� 5�      -   5        $ clubPhoneTextBox��5�      #��[ 5�      $��Phone��5�      5�      5�      �� 5�        ButtonsPlace_style#��[ 6   .+�5�      ��   costShowerPlace_style#��[ 6   ._�5�      �� -   5         ��Name$
 Cost��5�        costText_style#��[ 5�      $�� ��5�      5�      �� 5�      -   5        
���   � � Cursor$
 Hand��5�      #��[ 5�      ._�5�      �� #��[ 6    $��Расписание��6H   5�      �� 5�      -   5        
���   �$
 Hand��5�      #��[ 5�      ._�5�      �� #��[ 6    $��Заказать��6H   5�      5�      �� 5�         gamesInformationBorder_style#��[ 6   ._�5�      � .+�5�      ��   GamePodium_style#��[ 6   �� 5�      -   5        $ GamePictureSocket��5�        gamePictureStyle#��[ 68   ._�5�      �� -   5         ��Name$ FrontGamePicture��5�       5�      �� 5�        gameInformationBorder_style#��[ 6   ._�5�      �� .+�5�      ��   smallImportantText_style#��[ 6(   $��Название��6Z   �� 5�      -   5        $ GameName��5�   &     smallImportantTextBox_style#��[ 68   �� 5�      -   5        $ 
GameRating��5�   !   #��[ 5�   !   $��Рейтинг��5�   !   �� 5�      -   5        $ GameCoop��5�   !   #��[ 5�   !   $��Совместная��5�   !   5�      5�      �� 5�      -   5        $ leftClickButton��5�      $
��Left=�5�       �� CornerRadius$ 	20 0 0 20��5�        ButtonsLeftRight_style#��[ 5�      ._�5�      �� $��Picture\leftArrow.png��6    5�      �� 5�      -   5        $ rightClickButton��5�      $��Right=�5�      $ 	0 20 20 0��5�      #��[ 5�      ._�5�      �� $��Picture\rightArrow.png��6    5�      �� 5�      -   5        $ GameInformationSheild��5�      
����p��$ 20��5�      ._�5�      �� -    5        $	��150��5�      $��Center=�5�      #��[ 5�      ._�5�      �� #��[ 5�   !   $��Показать Игры��5�   !   $
��Wrap=�5�   !   5�      5�      5�      5�      5�      5�      5�      