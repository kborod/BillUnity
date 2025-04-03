namespace Kborod.UI.Screens
{
    public enum RemoveState
    {
		/**
		 * Шар движется на пути к точке лузы
		 */
		STATE_TO_POCKET_POINT = 1,
		/**
		 * Шар движется от точки лузы под стол
		 */
		STATE_FROM_POCKET = 2,
		/**
		 * Шар под столом
		 */
		STATE_HIDDEN = 3,
		/**
		 * Шар движется по тракектории приемника
		 */
		STATE_PATH_MOVE = 4,
		/**
		 * Шар в конечной точке
		 */
		STATE_ANIM_OVER = 5,
        /**
		 * Шар без анимации, ждет очереди для помещения в конечную точку. Отобразится в конечной точке после того, как окончатся все текущие анимации
		 */
        STATE_WITHOUT_ANIM_WAIT = 6	
    }
}
