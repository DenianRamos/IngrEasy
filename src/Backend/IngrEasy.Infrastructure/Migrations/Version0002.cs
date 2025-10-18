using FluentMigrator;
using IngrEasy.Infrastructure.Migrations.Versions;

namespace IngrEasy.Infrastructure.Migrations;
[Migration(DatabaseVersion.TABLE_RECIPES, "Create the User Recipes table")]

public class Version0002 : VersionBase
{
    const string RECIPE_TABLE_NAME = "Recipes";
    public override void Up()
    {
        CreateTable(RECIPE_TABLE_NAME)
            .WithColumn("Title").AsString(255).NotNullable()
            .WithColumn("CookingTime").AsInt32().Nullable()
            .WithColumn("Difficulty").AsInt32().Nullable()
            .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("FK_Recipes_User_id", "Users", "Id");


        CreateTable("Ingredients")
            .WithColumn("Item").AsString().NotNullable()
            .WithColumn("RecipeId").AsInt32().NotNullable().ForeignKey("FK_Ingredients_Recipe_id", RECIPE_TABLE_NAME, "Id")
            .OnDelete(System.Data.Rule.Cascade);
        
        CreateTable("Instructions")
            .WithColumn("Step").AsString().NotNullable()
            .WithColumn("Text").AsString(2000).NotNullable()
            .WithColumn("RecipeId").AsInt32().NotNullable().ForeignKey("FK_Instructions_Recipe_id", RECIPE_TABLE_NAME, "Id")
            .OnDelete(System.Data.Rule.Cascade);
        
        CreateTable("DishTypes")
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("RecipeId").AsInt32().NotNullable().ForeignKey("FK_DishTypes_Recipe_id", RECIPE_TABLE_NAME, "Id")
            .OnDelete(System.Data.Rule.Cascade);
    }
}