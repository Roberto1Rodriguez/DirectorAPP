﻿using DirectorAPP.Views;

namespace DirectorAPP;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        MainPage = new NavigationPage(new LoginView());
    }
}
