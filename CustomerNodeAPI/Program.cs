var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Branch> Branches;
List<Customer> Customers;
int DownstreamCustomers;

app.MapPost("/downstreamCustomers", (NetworkData data) =>
{
    ResponseData response = new ResponseData();
    response.DownstreamCustomers = ProcessData(data);
    return response;
});

app.Run();

int ProcessData(NetworkData data)
{
    var network = data.Network;
    Branches = network.Branches;
    Customers = network.Customers;
    DownstreamCustomers = 0;
    var selected = data.SelectedNode;

    CountCustomersAtNode(selected);

    return DownstreamCustomers;
}

void CountCustomersAtNode(int node)
{
    DownstreamCustomers += Customers.Where(c => c.Node == node).Select(c => c.NumberOfCustomers).Sum();
    var branchesAtNode = Branches.Where(b => b.StartNode == node).ToList();
    foreach (var branch in branchesAtNode)
        CountCustomersAtNode(branch.EndNode);
}