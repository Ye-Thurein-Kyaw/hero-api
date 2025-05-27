

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using heroApi.Models;
using heroApi.Repositories;

namespace heroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public TodoItemsController(IRepositoryWrapper RW)
        {
            _repositoryWrapper = RW;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
           
            var todoItems =  await _repositoryWrapper.TodoItem.FindAllAsync();
            return todoItems
                .Select(x => ItemToDTO(x))
                .ToList();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _repositoryWrapper.TodoItem.FindByIDAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }
        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _repositoryWrapper.TodoItem.FindByIDAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {
                await _repositoryWrapper.TodoItem.UpdateAsync(todoItem);
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };

        
            await _repositoryWrapper.TodoItem.CreateAsync(todoItem, true);

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                ItemToDTO(todoItem));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            if (_repositoryWrapper.TodoItem == null)
            {
                return NotFound();
            }
            var todoItem = await _repositoryWrapper.TodoItem.FindByIDAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

           
            await _repositoryWrapper.TodoItem.DeleteAsync(todoItem, true);

            return NoContent();
        }

        [HttpPost("search/{term}")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>>  SearchTodo(string term)
        {
            var empList = await _repositoryWrapper.TodoItem.SearchTodo(term);
            return Ok(empList);           
        }

        [HttpPost("searchtodo")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>>  SearchTodoMultiple(TodoSearchPayload SearchObj)
        {
            var empList = await _repositoryWrapper.TodoItem.SearchTodoMultiple(SearchObj);
            return Ok(empList);           
        }

        private bool TodoItemExists(long id)
        {
            return _repositoryWrapper.TodoItem.IsExists(id);
        }

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                Note = todoItem.Note
            };
    }
}