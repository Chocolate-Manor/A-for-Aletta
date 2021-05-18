using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CharacterTest
{
    private Character jean;
    private Sprite sprite8;
    private Sprite sprite16;
        
    [SetUp]
    public void Setup()
    {
        jean = ScriptableObject.CreateInstance<Character>();
        
        Texture2D texture16 = new Texture2D(16, 16);
        sprite16 = Sprite.Create(texture16, new Rect(0, 0, texture16.width, texture16.height),
            new Vector2(0.5f, 0.5f), 16);

        Texture2D texture8 = new Texture2D(8, 8);
        sprite8 = Sprite.Create(texture8, new Rect(0, 0, texture8.width, texture8.height),
            new Vector2(0.5f, 0.5f), 16);

        jean.portraits = new List<Sprite>();
        jean.portraitNameIndex = new List<string>();
        jean.textBoxes = new List<Sprite>();
        jean.textBoxNameIndex = new List<string>();
    }

    [Test]
    public void Get_The_Correct_Portrait()
    {
        jean.portraits.Add(sprite16);
        jean.portraits.Add(sprite8);
        jean.portraits.Add(sprite16);
        
        jean.portraitNameIndex.Add("sprite16");
        jean.portraitNameIndex.Add("sprite8");
        jean.portraitNameIndex.Add("sprite16Copy");
        
        Assert.AreEqual(16, jean.GetPortraitByName("sprite16").texture.width);
        Assert.AreEqual(8, jean.GetPortraitByName("sprite8").texture.width);
        Assert.AreEqual(16, jean.GetPortraitByName("sprite16Copy").texture.width);
    }

    [Test]
    public void Throw_Error_For_Trying_To_Get_NonExistent_Portrait()
    {
        jean.portraits.Add(sprite16);
        jean.portraits.Add(sprite8);
        
        jean.portraitNameIndex.Add("sprite16");
        jean.portraitNameIndex.Add("sprite8");
        
        Assert.That(() => jean.GetPortraitByName("sprite16Copy"), Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Throw_Error_If_PortraitList_And_PortraitIndex_Have_Unequal_Length()
    {
        jean.portraits.Add(sprite16);
        
        jean.portraitNameIndex.Add("sprite16");
        jean.portraitNameIndex.Add("sprite8");
        
        Assert.That(() => jean.GetPortraitByName("sprite16"), Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Get_The_Correct_TextBox()
    {
        jean.textBoxes.Add(sprite16);
        jean.textBoxes.Add(sprite8);
        jean.textBoxes.Add(sprite16);
        
        jean.textBoxNameIndex.Add("sprite16");
        jean.textBoxNameIndex.Add("sprite8");
        jean.textBoxNameIndex.Add("sprite16Copy");
        
        Assert.AreEqual(16, jean.GetTextBoxByName("sprite16").texture.width);
        Assert.AreEqual(8, jean.GetTextBoxByName("sprite8").texture.width);
        Assert.AreEqual(16, jean.GetTextBoxByName("sprite16Copy").texture.width);
    }
    
    [Test]
    public void Throw_Error_For_Trying_To_Get_NonExistent_TextBox()
    {
        jean.textBoxes.Add(sprite16);
        jean.textBoxes.Add(sprite8);
        
        jean.textBoxNameIndex.Add("sprite16");
        jean.textBoxNameIndex.Add("sprite8");
        
        Assert.That(() => jean.GetTextBoxByName("sprite16Copy"), Throws.TypeOf<ArgumentException>());
    }
    
    [Test]
    public void Throw_Error_If_TextBoxList_And_TextBoxIndex_Have_Unequal_Length()
    {
        jean.textBoxes.Add(sprite16);
        
        jean.textBoxNameIndex.Add("sprite16");
        jean.textBoxNameIndex.Add("sprite8");
        
        Assert.That(() => jean.GetTextBoxByName("sprite16"), Throws.TypeOf<InvalidOperationException>());
    }
    
}
