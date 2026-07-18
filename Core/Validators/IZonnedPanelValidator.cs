using Core.Layouts;

namespace Core.Validators;

public interface IZonnedPanelValidator
{
    ValidationResult Validate(IZonedPanel panel);
}