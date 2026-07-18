using Core.Layouts;

namespace Core.Validators;

public interface IOpeningPanelValidator
{
    ValidationResult Validate(IOpeningPanel panel, double step);
}
