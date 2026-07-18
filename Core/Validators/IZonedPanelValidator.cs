using Core.Layouts;

namespace Core.Validators;

public interface IZonedPanelValidator
{
    ValidationResult Validate(IZonedPanel panel);
}
