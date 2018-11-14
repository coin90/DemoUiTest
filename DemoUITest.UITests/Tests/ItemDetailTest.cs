﻿using System;
using DemoUITest.UITests.Pages;
using NUnit.Framework;
using Xamarin.UITest;

namespace DemoUITest.UITests.Tests
{
	public class ItemDetailTest : BaseTest
    {
        public ItemDetailTest(Platform platform) : base(platform)
        {
        }

        [Test]
        public void DetailItem()
        {

            var itemsPage = new ItemsPage();

            itemsPage.SelectFirstCellByAutomationId();

            new ItemDetailPage()
                .CheckLoadView();
        }
    }
}