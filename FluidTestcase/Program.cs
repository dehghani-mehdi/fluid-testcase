using System;
using System.Collections.Generic;
using System.Linq;

using Fluid;
using Fluid.Values;

namespace FluidTestcase
{
    class Program
    {
        const string LOREM = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";

        const string TEMPLATE = @"<ul id='products'>
	{% for p in products %}
    <li>
		<h2>{{ p.name }}</h2>
		<span>Only {{ p.price }}</span>
		<p>{{ p.description | truncate: 30 }}</p>
    </li>
	{% endfor %}
</ul>";

        static void Main(string[] args)
        {
            var products = new List<Product>();

            for (int i = 0; i < 1; i++) products.Add(new Product("Name" + i, i, LOREM));

            var parser = new FluidParser();

            parser.TryParse(TEMPLATE, out var template, out var _);

            var options = new TemplateOptions();
            options.MemberAccessStrategy.Register<Product>();
            options.MemberAccessStrategy.MemberNameStrategy = MemberNameStrategies.CamelCase;

            var context = new TemplateContext(options).SetValue("products", new ArrayValue(products.Select(p => new ObjectValue(p))));

            var result = template.Render(context);

            Console.WriteLine(result);
        }
    }

    public class Product
    {
        public Product(string name, float price, string description)
        {
            Name = name;
            Price = price;
            Description = description;
        }

        public string Name { get; set; }

        public float Price { get; set; }

        public string Description { get; set; }
    }
}