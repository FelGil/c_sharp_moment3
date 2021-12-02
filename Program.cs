using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace moment3 {

    //Post class to handle posts
    public class Post {
        private string usr;
        private string pst;
        //set variable user to Post Class
        public string user {
            set {this.usr = value;}
            get {return this.usr;}
        }
        //set variable post to Post Class
        public string post {
            set {this.pst = value;}
            get {return this.pst;}
        }
    }

    //Class to handle the guestbook and its functions.
    public class Guestbook {
        //Private varaible for questbook.json file
        private string filename  = @"guestbook.json";
        //Create a new Post from above class and set it as a list
        private List<Post> posts = new List<Post>();
        public Guestbook() {
            //Check if guestbook.json exist, if yes load content
            if(File.Exists(@"guestbook.json") == true) {
                //Get filecontent
                string jsonString = File.ReadAllText(filename);
                //Deserialize conetent and att it to the created list.
                posts = JsonSerializer.Deserialize<List<Post>>(jsonString);

            }
        }

        //Function to add new post.
        public Post addPost (string user, string post) {
            //Create a new post
            Post newPost = new Post();
            //Add passed through variabled to the new post.
            newPost.user = user;
            newPost.post = post;

            //Add post to the list
            posts.Add(newPost);
            //write the changes the file.
            toFile();
            return newPost;
        }
        //Function to delete post.
        public int delPost (int i) {
            //Run removeat command with given value to remove content form list.
            posts.RemoveAt(i);
            //write changes to the file.
            toFile();
            return i;
        }
        //get Posts
        public List<Post> GetPosts() {
            return posts;
        }
        //Function to write new content to file.
        public void toFile() {
            //Serialize the content and put in to variable jsonString.
            var jsonString = JsonSerializer.Serialize(posts);
            //Push the content of jsonString to the file.
            File.WriteAllText(filename, jsonString);
        }

    }
    class Program
    {
        //Function to remove content at the current row.
        public static void ClearCurrentConsoleLine() {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, currentLineCursor);
        }
        static void Main(string[] args)
        {
            //Create a new guestbook
            Guestbook guestbook = new Guestbook();
            //Variable to hold number of indexes in list
            int i;
            //Set text color to white
            Console.ForegroundColor = ConsoleColor.White;

            //Loop to always show the menu after you are done adding a new content or deleting it.
            while(true) {
                //Clear all content in terminal
                Console.Clear();
                //Remove the blinking cursor
                Console.CursorVisible = false;
                //Write out the menu
                Console.WriteLine("Välkommen till min Gästbok!\n\n");
                Console.WriteLine("[1] Lägg till ett nytt inlägg");
                Console.WriteLine("[2] Ta bort ett inlägg");
                Console.WriteLine("[x] Avsluta");

                i=0;
            
                Console.WriteLine("\n________________________________");
                Console.WriteLine("\nHär nedan dom som redan skrivit i gästboken");
                //Write out allposts
                foreach(Post post in guestbook.GetPosts()) {
                    Console.WriteLine($"[{i}] {post.user} - {post.post}");
                    i++;
                }
                //waiting for a key to be pressed
                int choice = (int) Console.ReadKey(true).Key;

                //Switch to handle the key input
                switch (choice) {
                    case '1':
                        //Show the cursor
                        Console.CursorVisible = true;
                        Console.WriteLine("\nLägg till nytt inlägg.");
                        
                        Console.Write("Ange ditt namn: ");
                        //Input user
                        String user = Console.ReadLine();
                        
                        //Check if user-input is empty, if empty the user will need to try again
                        while(String.IsNullOrEmpty(user)) {
                            //Move cursor up two lines
                            Console.SetCursorPosition(0, Console.CursorTop -2);
                            //Clear content at the new line
                            ClearCurrentConsoleLine();
                            Console.WriteLine("Lägg till nytt inlägg.");
                            //set color red for the error message
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Namn kan inte vara tomt. Försök igen");
                            //Turn the text to white again.
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Ange ditt namn: ");
                            user = Console.ReadLine();
                        }
                        
                        Console.Write("Skriv ditt inlägg: ");
                        String post = Console.ReadLine();
                        //Check if user-input is empty, if empty the user will need to try again
                        while(String.IsNullOrEmpty(post)) {
                            //Move cursor up one lines
                            Console.SetCursorPosition(0, Console.CursorTop -1);
                            //Clear content at the new line
                            ClearCurrentConsoleLine();
                            //set color red for the error message
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ditt inlägg kan inte vara tomt. Försök igen");
                            //Turn the text to white again.
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Skriv ditt inlägg: ");
                            post = Console.ReadLine();
                        }
                        //add the new post to the guestbook
                        guestbook.addPost(user, post);
                        break;

                    case '2':
                    //Show the cursor
                        Console.CursorVisible = true;
                        
                        Console.WriteLine("\nTa bort inlägg.");
                        Console.Write("Vilket inlägg vill du ta bort? ");
                        //input index on what row to delete.
                        string getIndex = Console.ReadLine();

                        //check if input is empty
                        while(String.IsNullOrEmpty(getIndex)) {
                            //Move cursor up two lines
                            Console.SetCursorPosition(0, Console.CursorTop -2);
                            //Clear content at the new line
                            ClearCurrentConsoleLine();
                            Console.WriteLine("Ta bort inlägg.");
                            //set color red for the error message
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Du måste ange ett värde, prova igen.");
                            //Turn the text to white again.
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Vilket inlägg vill du ta bort? ");
                            getIndex = Console.ReadLine();
                        }
                        //convert the index to integer.
                        int index = Convert.ToInt32(getIndex);
                        //check if the integer is in the range of the list.
                        while(index < 0 || index  > i - 1 ) {
                            //Move cursor up three lines
                            Console.SetCursorPosition(0, Console.CursorTop -3);
                            //Clear content at the new line
                            ClearCurrentConsoleLine();
                            Console.WriteLine("Ta bort inlägg.");
                            //set color red for the error message
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Det fanns inget inlägg med index {index}");
                            //Turn the text to white again.
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Vilket inlägg vill du ta bort? ");
                            index = Convert.ToInt32(Console.ReadLine()); 
                        }

                        //Delete the selected row
                        guestbook.delPost(index);
                        break;

                    case 88:
                        //Exit the application
                        Environment.Exit(0);
                        break;
                }
            
            }


        }
    }
}