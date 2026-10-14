namespace Desktop.Navigation
{
    /// <summary>
    /// Точка доступа к сервису навигации для моделей представления.
    /// </summary>
    public static class Navigator
    {
        public static INavigationService? Current { get; set; }
    }
}
