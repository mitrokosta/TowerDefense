using Assets;

namespace Enemy
{
    public class EnemyData
    {
        private EnemyView m_View;

        public EnemyView View => m_View;

        public EnemyData(EnemyAsset asset)
        {
            
        }

        public void AttachView(EnemyView view)
        {
            m_View = view;
            m_View.AttachData(this);
        }
    }
}