using Godot;
using System;

public class menuTabs : Control
{
	[Export]
	public ButtonGroup buttonGroup;

	public GridContainer hotBar;
	public GridContainer InventorySpace;

	public PlayerMenu PlayerMenu;
	public VBoxContainer questContainer;

	public override void _Ready()
	{
		PlayerMenu = (PlayerMenu)FindParent("PlayerMenu");
		InventorySpace = PlayerMenu.GetNode<GridContainer>("InventorySpace");
		hotBar = PlayerMenu.GetNode<GridContainer>("HotBar");
		questContainer = PlayerMenu.GetNode<VBoxContainer>("QuestContainer");

		foreach (TextureButton menutab in buttonGroup.GetButtons())
        {
			menutab.Connect("pressed", this, "menuTabPressed");

		}
		questContainer.Visible = false;
		show_inventory();

	}

	public void show_inventory()
    {
		hotBar.Visible = true;
		InventorySpace.Visible = true;

	}

	public void hide_inventory()
    {
		hotBar.Visible = false;
		InventorySpace.Visible = false;


	}

	public void show_Quest()
    {

		questContainer.Visible = true;

	}

	public void hide_Quest()
	{

		questContainer.Visible = false;

	}


	public void menuTabPressed()
    {
		string tab_name = buttonGroup.GetPressedButton().Name;

		switch (tab_name)
        {
			case "InventoryTab":
				show_inventory();
				hide_Quest();
				GD.Print("show inventory");
				break;

			case "MapTab":
				hide_inventory();
				hide_Quest();
				GD.Print("show map");
				break;

			case "QuestTab":
				hide_inventory();
				show_Quest();
				GD.Print("show Quest");
				break;
			
			case "RelationShipTab":
				GD.Print("show Relationships");
				break;
		}
    }





}
