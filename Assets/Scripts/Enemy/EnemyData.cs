using Assets;

namespace Enemy
{
    public class EnemyData
    {
        private EnemyView m_View;
        public readonly EnemyAsset Asset;

        public EnemyView View => m_View;

        public EnemyData(EnemyAsset asset)
        {
            this.Asset = asset;
        }

        public void AttachView(EnemyView view)
        {
            m_View = view;
            m_View.AttachData(this);
        }
    }
}