using Microsoft.Extensions.DependencyInjection;
using PnP.Core.Model.SharePoint;
using PnP.Core;
using PnP.Core.QueryModel;
using PnP.Core.Services;
using PnPCoreLargeLists;
using System.Drawing.Printing;

async Task GetItemsFromLargeList()
{
    try
    {
	   var host = await Auth.Initialize();
	   using(var scope = host.Services.CreateScope())
	   {
		  var pnpContextFactory = scope.ServiceProvider.GetRequiredService<IPnPContextFactory>();
		  using(var ctx = await pnpContextFactory.CreateAsync("Dev"))
		  {
                Console.WriteLine("");
                Console.WriteLine("Getting the web information!");
                Console.WriteLine("");
                var web = await ctx.Web.GetAsync(p => p.Title, p => p.Id,
                            p => p.ContentTypes.QueryProperties(p => p.Name),
                            p => p.Lists.QueryProperties(p => p.Id, p => p.Title, p => p.DocumentTemplate));

                List<IListItem> targetCases = null;
                Console.WriteLine("");
                Console.WriteLine($"Web Title - {web.Title}, ID - {web.Id}");
                Console.WriteLine("");

                string strTargetList = "CIF Master";
                string qry_items = string.Empty;

                qry_items = @"<View>
                                <Query>				                        
                                    <OrderBy>
                                        <FieldRef Name='ID' Ascending='TRUE'/>
                                    </OrderBy>
                                </Query>
                                <ViewFields>
                                    <FieldRef Name='ID'/>
                                </ViewFields>			                            
                            </View>";
                Console.WriteLine("");
                Console.WriteLine("Trying to get total items without row limit in CAML Query");
                Console.WriteLine($"Query used: {qry_items}");
                Console.WriteLine("");
                try
                {
                    targetCases = await GetListDataWithCamlQuery(ctx, strTargetList, qry_items);
                    Console.WriteLine($"Total No. of items without row limit in CAML Query: {targetCases.Count.ToString()}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("");
                }
                
                Console.WriteLine("----------------");

                qry_items = @"<View>
                                <Query>				                        
                                    <OrderBy>
                                        <FieldRef Name='ID' Ascending='TRUE'/>
                                    </OrderBy>
                                </Query>
                                <ViewFields>
                                    <FieldRef Name='ID'/>
                                </ViewFields>	
                                <RowLimit>2000</RowLimit>
                            </View>";
                Console.WriteLine("");
                Console.WriteLine("Trying to total items with row limit in CAML query");
                Console.WriteLine($"Query used: {qry_items}");
                Console.WriteLine("");
                try
                {
                    targetCases = await GetListDataWithCamlQuery(ctx, strTargetList, qry_items);
                    Console.WriteLine($"Total No. of items with row limit in CAML Query: {targetCases.Count.ToString()}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("");
                }                
                Console.WriteLine("----------------");

                qry_items = @"<View>
			                 <Query>				                        
				                <OrderBy>
				                    <FieldRef Name='ID' Ascending='TRUE'/>
				                </OrderBy>
			                 </Query>
			                 <ViewFields>
				                <FieldRef Name='ID'/>
			                 </ViewFields>	
                                <RowLimit Paged='TRUE'>2000</RowLimit>
		                  </View>";
                Console.WriteLine("");
                Console.WriteLine("Trying to get total items with row limit and paging in caml query");
                Console.WriteLine($"Query used: {qry_items}");
                Console.WriteLine("");
                try
                {
                    targetCases = await GetListDataWithCamlQuery(ctx, strTargetList, qry_items);
                    Console.WriteLine($"Total No. of items with row limit and paging in CAML Query: {targetCases.Count.ToString()}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("");
                }                
                Console.WriteLine("--------------");

                qry_items = @"<View>
			                 <Query>	
                                    <Where>
				                    <Eq>
					                   <FieldRef Name='CIFStatus'/>
					                   <Value Type='Text'>Active</Value>
				                    </Eq>
				                </Where>
				                <OrderBy>
				                    <FieldRef Name='ID' Ascending='TRUE'/>
				                </OrderBy>
			                 </Query>
			                 <ViewFields>
				                <FieldRef Name='ID'/>
			                 </ViewFields>	
		                  </View>";
                Console.WriteLine("");
                Console.WriteLine("Trying to filter items without row limit in caml query.");
                Console.WriteLine($"Query used: {qry_items}");
                Console.WriteLine("");
                try
                {
                    targetCases = await GetListDataWithCamlQuery(ctx, strTargetList, qry_items);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("");
                }
                Console.WriteLine($"Finding an item with the filter without row limit in CAML Query: {targetCases.Count.ToString()}");
                Console.WriteLine("------------------");

                qry_items = @"<View>
			                 <Query>	
                                    <Where>
				                    <Eq>
					                   <FieldRef Name='CIFStatus'/>
					                   <Value Type='Text'>Active</Value>
				                    </Eq>
				                </Where>
				                <OrderBy>
				                    <FieldRef Name='ID' Ascending='TRUE'/>
				                </OrderBy>
			                 </Query>
			                 <ViewFields>
				                <FieldRef Name='ID'/>
			                 </ViewFields>
                                <RowLimit>2000</RowLimit>
		                  </View>";
                Console.WriteLine("");
                Console.WriteLine("Trying to filter items with row limit in caml query.");
                Console.WriteLine($"Query used: {qry_items}");
                Console.WriteLine("");
                try
                {
                    targetCases = await GetListDataWithCamlQuery(ctx, strTargetList, qry_items);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("");
                }
                Console.WriteLine($"Finding an item with the filter with row limit in CAML Query: {targetCases.Count.ToString()}");
                Console.WriteLine("-----------------");

                qry_items = @"<View>
			                 <Query>	
                                    <Where>
				                    <Eq>
					                   <FieldRef Name='CIFStatus'/>
					                   <Value Type='Text'>Active</Value>
				                    </Eq>
				                </Where>
				                <OrderBy>
				                    <FieldRef Name='ID' Ascending='TRUE'/>
				                </OrderBy>
			                 </Query>
			                 <ViewFields>
				                <FieldRef Name='ID'/>
			                 </ViewFields>
                                <RowLimit Paged='TRUE'>2000</RowLimit>
		                  </View>";
                Console.WriteLine("");
                Console.WriteLine("Trying to filter items with row limit and paging in caml query");
                Console.WriteLine($"Query used: {qry_items}");
                Console.WriteLine("");
                try
                {
                    targetCases = await GetListDataWithCamlQuery(ctx, strTargetList, qry_items);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("");
                }
                Console.WriteLine($"Finding an item with the filter with row limit and paging in CAML Query: {targetCases.Count.ToString()}");
                Console.WriteLine("----------------");
            }
	   }
    }
    catch (Exception ex)
    {
	   Console.WriteLine(ex.ToString());
    }
}

async Task<List<IListItem>> GetListDataWithCamlQuery(PnPContext ctx, string listName, string camlQuery)
{
    List<IListItem> retItems = new List<IListItem>();
    try
    {
        var myList = ctx.Web.Lists.GetByTitle(listName, p => p.Title,
                                                 p => p.Fields.QueryProperties(p => p.InternalName,
                                                                               p => p.FieldTypeKind,
                                                                               p => p.TypeAsString,
                                                                               p => p.Title));
        // Load all the needed data using paged requests
        bool paging = true;
        string nextPage = null;
        while (paging)
        {
            var output = await myList.LoadListDataAsStreamAsync(new RenderListDataOptions()
            {
                ViewXml = camlQuery,
                RenderOptions = RenderListDataOptionsFlags.ListData,
                Paging = nextPage ?? null,
                DatesInUtc = true
            }).ConfigureAwait(false);
            if (output.ContainsKey("NextHref"))
            {
                nextPage = output["NextHref"].ToString().Substring(1);
            }
            else
            {
                paging = false;
            }
            retItems = retItems.Union(myList.Items.AsRequested().ToList()).ToList();
        }
    }
    catch (PnPException ex)
    {
        Console.WriteLine(ex.ToString());
    }
    return retItems;
}

await GetItemsFromLargeList();

Console.WriteLine("All done...");
Console.ReadLine();

