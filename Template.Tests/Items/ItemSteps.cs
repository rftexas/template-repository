
using Bogus;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Reqnroll;
using Template.Core.Items;
using Template.Persistence;

namespace Template.Tests.Items;

[Binding]
public class ItemSteps
{
    private string _itemPayload = string.Empty;
    private DataContext _dataContext = null!;
    private Result<Domain.Items.Item> _result = null!;

    [BeforeScenario]
    public void BeforeScenario()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        _dataContext = new DataContext(options);
        _dataContext.Database.OpenConnection();
        _dataContext.Database.EnsureCreated();
    }

    [AfterScenario]
    public void AfterScenario()
    {
        _dataContext.Database.CloseConnection();
        _dataContext.Dispose();
    }

    [Given("I have a template with name {string}")]
    public void GivenIHaveItemWithName(string itemName)
    {
        _dataContext.Items.Add(Domain.Items.Item.Create(itemName));
        _dataContext.SaveChanges();
    }

    [Given("I have a valid template payload")]
    public void GivenIHaveAValidItemPayload()
    {
        _itemPayload = new Faker().Name.JobTitle();
    }

    [Given("I have an invalid template payload")]
    public void GivenIHaveAnInvalidItemPayload()
    {
        _itemPayload = string.Empty;
    }

    [Given("the template does not already exist")]
    public async Task GivenTheItemDoesNotAlreadyExist()
    {
        await _dataContext.Items.Where(i => i.ItemName == _itemPayload).ExecuteDeleteAsync();
    }

    [Given("the template already exists")]
    public async Task GivenTheItemAlreadyExists()
    {
        await _dataContext.Items.AddAsync(Domain.Items.Item.Create(_itemPayload));
        await _dataContext.SaveChangesAsync();
    }

    [When("I send a request to add the template")]
    public async Task WhenISendARequestToAddTheItem()
    {
        var handler = new AddItem.Handler(_dataContext);
        _result = await handler.Handle(new AddItem.Command(_itemPayload));
    }

    [Then("the template should be successfully added")]
    public void ThenTheItemShouldBeSuccessfullyAdded()
    {
        Assert.True(_result.IsSuccess);
    }

    [Then("the template should not be added")]
    public void ThenTheItemShouldNotBeAdded()
    {
        Assert.True(_result.IsFailed);
    }
}