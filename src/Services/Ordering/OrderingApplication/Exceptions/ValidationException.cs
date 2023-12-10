using FluentValidation.Results;

namespace OrderingApplication.Exceptions;
public class ValidationException : ApplicationException
{
    public ValidationException() : base("one or more validation failures have occurred ")
    {
        Errors = new Dictionary<string,string[]>();
    }
    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
    public Dictionary<string,string[]> Errors {get;}
}
