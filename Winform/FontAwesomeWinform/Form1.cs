using FontAwesome.Sharp;
namespace FontAwesomeWinform;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        // 创建图标按钮
        IconButton iconButton = new IconButton();
        iconButton.IconChar = IconChar.Home;
        iconButton.Text = "    Home";
        iconButton.IconSize = 32;
        iconButton.Size = new Size(128, 32);
        iconButton.Location = new Point(0, 0);
        Controls.Add(iconButton);

        // 创建图标
        IconPictureBox iconPicture = new IconPictureBox();
        iconPicture.IconChar = IconChar.Search;
        iconPicture.Size = new Size(32, 32);
        iconPicture.Location = new Point(200, 0);
        Controls.Add(iconPicture);
    }
}
