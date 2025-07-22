using UnityEngine;

namespace MH.EntitySystem
{
    public abstract class EntityComponent : MonoBehaviour
    {
        protected BaseEntity _entity;

        public virtual void Initialized(BaseEntity baseEntity)
        {
            _entity = baseEntity;
        }

        public virtual void ManualUpdate() { }
        public virtual void ManualFixedUpdate() { }
    }
}
