namespace Assets.Scripts.UI.Selectables
{
    internal interface ISelectable
    {
        bool IsSeleceted();
        void Select();
        void Deselect();
    }
}
