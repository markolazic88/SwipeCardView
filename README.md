## SwipeCardView Control for Xamarin Forms

SwipeCardView is a lightweight MVVM friendly user control that brings Tinder-style swipe card view into Xamarin.Forms applications.

[![NuGet](https://img.shields.io/nuget/v/MLToolkit.Forms.SwipeCardView.svg?label=NuGet)](https://www.nuget.org/packages/MLToolkit.Forms.SwipeCardView/)

## Features: 
- `ItemSource` and `ItemTemplate` support
- `SwipedLeftCommand` and `SwipedLeftCommand` support
- **Any** View can be used as a cell
- **All** Xamarin.Forms platforms supported
 
<img alt="" src="http://i68.tinypic.com/2h5psw9.png"  width="200px" /> <img alt="" src="http://i66.tinypic.com/2nh2lp4.png" width="200px"/>

<img alt="" src="http://i68.tinypic.com/ngbtdu.png"  width="200px" /> <img alt="" src="http://i65.tinypic.com/2e4g1vn.png" width="200px" />

## XAML Usage

Example of how control should be used in XAML of your application:

```XML
<swipeCardView:SwipeCardView
                            ItemsSource="{Binding ViewModelItems}"
                            SwipedLeftCommand="{Binding SwipedLeftCommand}"
                            SwipedRightCommand="{Binding SwipedRightCommand}"
                            TopItem="{Binding TopItem}">
     <swipeCardView:SwipeCardView.ItemTemplate>
          <DataTemplate x:Name="SomeTemplate">
		<!-- Template -->
          </DataTemplate>
      </swipeCardView:SwipeCardView.ItemTemplate>
</swipeCardView:SwipeCardView>
```

ViewModelItems is an ObrvableCollection defined in your ViewModel
TopItem is observable property that should have the same type like the elements of the collection
Template represent how each card should look like

## Example:

Full working example can be found on my GitHub repository. It’s an image gallery app called [DailyCat](https://github.com/markolazic88/DailyCat).

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

For a detailed explanation of design, implementation and usage of this library, check my blog post: [Create Tinder-like UI in Xamarin Forms using SwipeCardView](https://markolazic.com/swipecardview-tinder-ui-xamarin-forms/).
