namespace DirectorAPP.Views;

public partial class AggUsuarioView : ContentPage
{
    int count = 0;
    public AggUsuarioView()
	{
		InitializeComponent();
	}

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
		count++;
		if (count == 1)
		{
            btnpass.IsPassword = false;
			imgbtn.Source = "visible.png";
        }
		else
		{
			btnpass.IsPassword = true;
			count = 0;
			imgbtn.Source = "ojo.png";
		}
	
    }
}