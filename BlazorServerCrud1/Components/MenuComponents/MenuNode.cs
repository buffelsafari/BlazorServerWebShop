using System.Diagnostics;

namespace BlazorServerCrud1.Components.MenuComponents
{
    

    public class MenuNode  // todo put somewhere else
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string Link
        {
            get 
            {
                return (Parent!=null?Parent.Link:"")+"/"+Name;
            }
        }
        

        public List<MenuNode> Children { get; set; } = new List<MenuNode>();

        public MenuNode? Parent { get; set; }

        public MenuNode(string name, MenuNode? parent)
        {
            Name = name;            
            Parent = parent;
        }

        public bool Add(string name, string path)
        {
            if (name == Name)
            {
                string[] split = path.Split('/', 2);
                //Debug.WriteLine("name hit");
                foreach (MenuNode child in Children)
                {
                    if (child.Name == split[0])
                    {
                        //Debug.WriteLine("hit on childName");
                        if (!String.IsNullOrEmpty(split[1]))
                        {
                            child.Add(split[0], split[1]);
                        }
                        return true;
                    }                    
                }
                

                //Debug.WriteLine("adding new child");
                MenuNode node = new MenuNode(split[0], this);                
                Children.Add(node);
                if (!String.IsNullOrEmpty(split[1]))
                {
                    node.Add(split[0], split[1]);
                }                
                
            }

            return false;

        }

        


    }
}
