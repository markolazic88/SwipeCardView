## SwipeCardView Control for Xamarin Forms

Simple cross platform which brings Tinder-style swipe card view into Xamarin.Forms applications.

NuGet: https://www.nuget.org/packages/MLToolkit.Forms.SwipeCardView/

<img alt="" src="http://i68.tinypic.com/2h5psw9.png"  width="200px" /> <img alt="" src="http://i66.tinypic.com/2nh2lp4.png" width="200px"/>

<img alt="" src="http://i68.tinypic.com/ngbtdu.png"  width="200px" /> <img alt="" src="http://i65.tinypic.com/2e4g1vn.png" width="200px" />

## Simple Example:

```XML
<swipeCardView:SwipeCardView
                            ItemsSource="{Binding Cats}"
                            SwipedLeftCommand="{Binding SwipedLeftCommand}"
                            SwipedRightCommand="{Binding SwipedRightCommand}"
                            TopItem="{Binding TopCat}">
            <swipeCardView:SwipeCardView.ItemTemplate>
                <DataTemplate x:Name="CatTemplate">
                    <Grid InputTransparent="True">
                        <Grid.RowDefinitions>
                            <RowDefinition Height="*"></RowDefinition>
                            <RowDefinition Height="50"></RowDefinition>
                        </Grid.RowDefinitions>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="*"></ColumnDefinition>
                            <ColumnDefinition Width="*"></ColumnDefinition>
                            <ColumnDefinition Width="*"></ColumnDefinition>
                        </Grid.ColumnDefinitions>
                        <Image Grid.Row="0" Grid.Column="0" Grid.ColumnSpan="3" Source="{Binding ImageSource}" Aspect="AspectFill" VerticalOptions="FillAndExpand" HorizontalOptions="FillAndExpand"></Image>

                        <StackLayout Grid.Row="1" Grid.Column="0" Orientation="Horizontal" Spacing="10" HorizontalOptions="Center">
                            <Image Source="{x:Static resources:Images.Dislike}" WidthRequest="32" HeightRequest="32" VerticalOptions="Center"></Image>
                            <Label Text="{Binding DislikeCount}" FontSize="20" VerticalOptions="Center"></Label>
                        </StackLayout>
                        <StackLayout Grid.Row="1" Grid.Column="2" Orientation="Horizontal" Spacing="10" HorizontalOptions="Center">
                            <Image Source="{x:Static resources:Images.Like}" WidthRequest="32" HeightRequest="32" VerticalOptions="Center"></Image>
                            <Label Text="{Binding LikeCount}" FontSize="20" VerticalOptions="Center"></Label>
                        </StackLayout>
                    </Grid>
                </DataTemplate>
            </swipeCardView:SwipeCardView.ItemTemplate>
        </swipeCardView:SwipeCardView>
```
