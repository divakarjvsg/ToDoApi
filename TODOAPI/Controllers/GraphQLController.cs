﻿using System;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models.GraphQl;



namespace TodoAPI.Controllers
{
    [Route("[controller]")]
    
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQlquery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs
            };

            var result = await _documentExecuter
                .ExecuteAsync(executionOptions);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
