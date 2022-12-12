using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;

namespace Bingo
{
    class Program
    {
        private static RelationshipGraph rg;

        // Read RelationshipGraph whose filename is passed in as a parameter.
        // Build a RelationshipGraph in RelationshipGraph rg
        private static void ReadRelationshipGraph(string filename)
        {
            rg = new RelationshipGraph();                           // create a new RelationshipGraph object

            string name = "";                                       // name of person currently being read
            int numPeople = 0;
            string[] values;
            Console.Write("Reading file " + filename + "\n");
            try
            {
                string input = System.IO.File.ReadAllText(filename);// read file
                input = input.Replace("\r", ";");                   // get rid of nasty carriage returns 
                input = input.Replace("\n", ";");                   // get rid of nasty new lines
                string[] inputItems = Regex.Split(input, @";\s*");  // parse out the relationships (separated by ;)
                foreach (string item in inputItems) 
		{
                    if (item.Length > 2)                            // don't bother with empty relationships
                    {
                        values = Regex.Split(item, @"\s*:\s*");     // parse out relationship:name
                        if (values[0] == "name")                    // name:[personname] indicates start of new person
                        {
                            name = values[1];                       // remember name for future relationships
                            rg.AddNode(name);                       // create the node
                            numPeople++;
                        }
                        else
                        {               
                            rg.AddEdge(name, values[1], values[0]); // add relationship (name1, name2, relationship)

                            // handle symmetric relationships -- add the other way
                            if (values[0] == "hasSpouse" || values[0] == "hasFriend")
                                rg.AddEdge(values[1], name, values[0]);

                            // for parent relationships add child as well
                            else if (values[0] == "hasParent")
                                rg.AddEdge(values[1], name, "hasChild");
                            else if (values[0] == "hasChild")
                                rg.AddEdge(values[1], name, "hasParent");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Unable to read file {0}: {1}\n", filename, e.ToString());
            }
            Console.WriteLine(numPeople + " people read");
        }

        // Show the relationships a person is involved in
        private static void ShowPerson(string name)
        {
            GraphNode n = rg.GetNode(name);
            if (n != null)
                Console.Write(n.ToString());
            else
                Console.WriteLine("{0} not found", name);
        }

        // Show a person's friends
        private static void ShowFriends(string name)
        {
            GraphNode n = rg.GetNode(name);
            if (n != null)
            {
                Console.Write("{0}'s friends: ", name);
                List<GraphEdge> friendEdges = n.GetEdges("hasFriend");
                foreach (GraphEdge e in friendEdges) {
                    Console.Write("{0} ", e.To());
                }
                Console.WriteLine();
            }
            else
                Console.WriteLine("{0} not found", name);     
        }

        // Show all the orphans
        private static void ShowOrphans()
        {
            Console.Write("Orphan(s): ");

            // Checks every node of the tree from the current RelationalGraph
            foreach (GraphNode i in rg.nodes)
            {
                // Nodes without a parent
                if (i.GetEdges("hasParent").Count == 0)
                {
                    Console.Write(i.Name + "\n");
                }
            }
        }

        // Show all the siblings for one person
        private static void ShowSiblings(string name)
        {

        }

        // show all the descendants for one person
        private static void ShowDescendants(string name)
        {
            // Set the root to be the name of the selected individual from RelationshipGraph
            GraphNode root = rg.GetNode(name);              
            
            // Selected individual is not in the current RelationalshipGraph
            if (root == null)
            {
                Console.Write(name + " is not found in the graph\n");
            }

            // Selected individual has no children
            else if (root.GetEdges("hasChild").Count == 0)
            {
                Console.Write(name + " has no children");
            }

            // Selected individual has descendants
            else
            {
                // Get the edges with hasChild of the root of the selected individual
                List<GraphEdge> descendants = root.GetEdges("hasChild");        

                // A list to store the current and next descendants
                List<GraphNode> current_descendants = new List<GraphNode>();             
                List<GraphNode> next_descendants = new List<GraphNode>();            

                // Set the generation of root to determine how many generation selected individual has
                int generation = 0;
                
                // For every edge with hasChild of the selected individual, add the node that the edge is pointing to current_descendants
                foreach (GraphEdge edge in descendants)
                {
                    current_descendants.Add(rg.GetNode(edge.To()));
                }

                while (current_descendants.Count >= 1)                                
                {

                    // Output the children of selected individual first
                    if (generation == 0)
                    {
                        Console.Write(root.GetName() + "'s Children:\n");
                    }

                    // Then output the grandchildren of selected individual
                    else if (generation == 1)
                    {
                        Console.Write(root.GetName() + "'s Grandchildren:\n");
                    }

                    // Then output the next grandchildren of selected individual
                    else
                    {
                        Console.Write(root.GetName() + "'s ");
                        for (int i = 0; i < generation; i++)
                        {
                            Console.Write("Great ");
                        }
                        Console.Write("GrandChildren:\n");

                    }

                    // Outputs the selected descendants of the selected individual in the current_descendants list
                    foreach (GraphNode descendant in current_descendants)
                    {
                        // Output the name of the selected descendant
                        Console.Write(descendant.GetName() + "\n");

                        // Get the edges with hasChild of the next individual
                        descendants = descendant.GetEdges("hasChild");

                        // For every edge with hasChild of the next individual, add the node that the edge is pointing to next_descendants
                        foreach (GraphEdge next_edge in descendants)            
                        {
                            next_descendants.Add(rg.GetNode(next_edge.To()));    
                        }
                    }

                    // Set the next_descendant list to current_decendant list to be outputted
                    current_descendants = next_descendants;                       
                    
                    // Delete the next_descendant list
                    next_descendants = new List<GraphNode>();                    
                    
                    // Increment generation to output the next descendants of selected individual
                    generation++;                                         
                }
            }
        }

        // Show bingo
        private static void ShowBingo(string start_name, string end_name)
        {
            GraphNode start_node = rg.GetNode(start_name);
            GraphNode end_node = rg.GetNode(end_name);

            if (start_node != null && end_node != null)
            {
                Hashtable visitedEdges = new Hashtable();
                Queue<GraphNode> nodeBFS = new Queue<GraphNode>();
                nodeBFS.Enqueue(start_node);
                while (nodeBFS.Count != 0 && nodeBFS.Peek() != end_node)
                {
                    GraphNode tempNode = nodeBFS.Dequeue();
                    List<GraphEdge> childEdges = tempNode.GetEdges();
                    foreach (GraphEdge edge in childEdges)
                    {
                        if (!visitedEdges.Contains(edge.To()))
                        {
                            visitedEdges.Add(edge.ToNode(), tempNode);      // keeps track of the spanning tree via edges
                            nodeBFS.Enqueue(edge.ToNode());
                        }
                    }
                }
                if (nodeBFS.Count >= 1 && nodeBFS.Peek() == end_node)
                {
                    GraphNode currentNode = end_node;
                    GraphNode parentNode = end_node;
                    Stack<string> relationshipStack = new Stack<string>();
                    while (currentNode != start_node)
                    {
                        parentNode = (GraphNode)visitedEdges[currentNode];
                        List<GraphEdge> parentEdges = parentNode.GetEdges();
                        foreach (GraphEdge graphEdge in parentEdges)
                        {
                            if (graphEdge.ToNode() == currentNode)
                            {
                                relationshipStack.Push(graphEdge.ToString());
                                break;
                            }
                        }
                        currentNode = parentNode;
                    }
                    while (relationshipStack.Count != 0)
                        Console.Write(relationshipStack.Pop() + "\n");
                }
                else
                {
                    Console.WriteLine("No relationship found between {0} and {1}", start_name, end_name);
                }
            }
            else
                Console.WriteLine("{0} and/or {1} were not found", start_name, end_name);
        }

        // Show cousins n k
        //private static void ShowCousins(string name)
        //{

        //}

        // accept, parse, and execute user commands
        private static void CommandLoop()
        {
            string command = "";
            string[] commandWords;
            Console.Write("Welcome to Benedicto's Dutch Bingo Parlor!\n");

            while (command != "exit")
            {
                Console.Write("\nEnter a command: ");
                command = Console.ReadLine();
                commandWords = Regex.Split(command, @"\s+");        // split input into array of words
                command = commandWords[0];

                if (command == "exit")
                {
                    ;                                               // do nothing
                }

                // read a relationship graph from a file
                else if ((command == "read" || command == "r") && commandWords.Length > 1)
                    ReadRelationshipGraph(commandWords[1]);

                // show information for one person
                else if ((command == "show" || command == "s") && commandWords.Length > 1)
                    ShowPerson(commandWords[1]);

                // show all friends for one person
                else if ((command == "friends" || command == "f") && commandWords.Length > 1)
                    ShowFriends(commandWords[1]);

                // dump command prints out the graph
                else if (command == "dump")
                    rg.Dump();

                // show all orphans
                else if (command == "orphans" || command == "o")
                {
                    ShowOrphans();
                }

                // show all siblings for one person
                else if ((command == "siblings" || command == "s") && commandWords.Length > 1)
                {
                    ShowSiblings(commandWords[1]);
                }

                // show all descendants for one person
                else if ((command == "descendants" || command == "d") && commandWords.Length > 1)
                {
                    ShowDescendants(commandWords[1]);
                }

                // show bingo 
                else if (command == "bingo" || command == "b")
                {
                    ShowBingo(commandWords[1], commandWords[2]);
                }

                // illegal command
                else
                    Console.Write("\n" + "Legal commands: " +
                        "read [filename], r [filename], dump, show [personname], friends [personname], \n " +
                        "f [personname], orphans, o, siblings [personname], s [personname]" +
                        "descendants [personname], d [personname], bingo [firstpersonname] [secondpersonname], b [firstpersonname] [secondpersonname], exit\n");
            }
        }

        static void Main(string[] args)
        {
            CommandLoop();
        }
    }
}
