* LEFT OFF => 
	* Need to configure IsCompleted when user clicks on list item
	* Need to configure delete endpoint when user wants to delete ToDo
* Enhance error messaging
	* Input form should be red
	* Test what happens when multiple errors happen, such as max length and valid email
* It would be best to clear any changes if the user doesn't save
* Would be nice to add sorting ... probably don't need to add filtering
* Figure out how to do spinner on htmx request. This would be much better than disabling
* Convert razor Entity property to use the Dto types. I think this is what I want to do. Might need to create a mapping profile or something too
* Remove unused endpoints such as /auth
* Setup logging. UpdateToDo.cs
* I feel like adding a table to track ToDo priorities might be nice
* When getting all the types, this is NOT really ideal. It gets everything lol. See if I can narrow this down. 2 places call this I think
* At the very end, I can maybe just delete the migrations folder and have 1 migration. Might be confusing though not sure
* Maybe come back and create a UpdateEntity class. Use in ToDoForm
* ToDo: DatabaseContext.ConfigureConventions seems like a good spot to put my custom model building logic
* ToDo: Probably a good idea to add SortOrder. Even though I really don't want to add drag and drop functionality, just adding SortOrder will be nice to ToDo and ToDoItem
* Add support for recurring ToDos
* I'll want to create EntityUpdater. Use in UpdateToDo.cs and the quick update endpoints