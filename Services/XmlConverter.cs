using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ToDoDAL.Model;
using ToDoDAL.Abstract;
using ToDoDAL.Concrete;


namespace Services
{
    public class GroupXmlConverter
    {
        private string Path => @"C:\Programs\DbToXml.xml";
        private readonly IRepository<Group> _repository;

        public GroupXmlConverter()
        {
            _repository = new EntityRepository<Group>();
        }

        public XElement CreateXElement(IEnumerable<Group> elements)
        {
            XNamespace ns = "http://myapi.ns";
            var elem = new XElement(ns + "Groups", 
                from c in elements.ToList()
                let todos = from t in c.ToDoList
                            select new XElement(ns + "Todo",
                            new XElement(ns + "TodoId",new XAttribute("id",t.NoteId)),
                            new XElement(ns + "TodoName",t.Name),
                            new XElement(ns + "TodoStatus",t.StatusId))
                select new XElement(ns + "Group",new XAttribute("id", c.GroupId), 
                new XElement(ns + "Name",c.Name),
                new XElement(ns + "UserId",c.UserId),
                todos == null? null :
                new XElement(ns + "TodoCollection",todos))               
                );
            return elem;
        }

        public void SaveGroupXml(XElement element)
        {
            if (element != null)
            {
                try
                {
                    element.Save(Path);
                }
                catch (Exception ex)
                {
                    throw new Exception("Cannot save file", ex.InnerException);
                }                
            }
        }
    }
}
