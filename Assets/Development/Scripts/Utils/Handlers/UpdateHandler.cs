using Action = System.Action;
using System.Collections;
using UnityEngine;

namespace GameHandler
{
    public class UpdateHandler : MonoBehaviour
    {
        private event Action OnUpdate;
        private event Action OnFixedUpdate;
        private event Action OnLateUpdate;

        private event Action OnOptimizedUpdate;

        [SerializeField] private float _optimizeUpdateTime = 0.1f;

        private void Start() => StartCoroutine(OptimizedUpdate());
        private void Update() => OnUpdate?.Invoke();
        private void FixedUpdate() => OnFixedUpdate?.Invoke();
        private void LateUpdate() => OnLateUpdate?.Invoke();

        public void AddUpdate(Action actionUpdate) => OnUpdate += actionUpdate;
        public void RemoveUpdate(Action actionUpdate) => OnUpdate -= actionUpdate;

        public void AddFixedUpdate(Action actionUpdate) => OnFixedUpdate += actionUpdate;
        public void RemoveFixedUpdate(Action actionUpdate) => OnFixedUpdate -= actionUpdate;

        public void AddLateUpdate(Action actionUpdate) => OnLateUpdate += actionUpdate;
        public void RemoveLateUpdate(Action actionUpdate) => OnLateUpdate -= actionUpdate;

        public void AddOptiomizeUpdate(Action actionUpdate) => OnOptimizedUpdate += actionUpdate;
        public void RemoveOptiomizeUpdate(Action actionUpdate) => OnOptimizedUpdate -= actionUpdate;

        public void startCoroutine(IEnumerator enumerator) => StartCoroutine(enumerator);
        public void stopCoroutine(IEnumerator enumerator) => StopCoroutine(enumerator);

        private IEnumerator OptimizedUpdate()
        {
            yield return new WaitForSeconds(_optimizeUpdateTime);

            OnOptimizedUpdate?.Invoke();

            StartCoroutine(OptimizedUpdate());
        }
    }
}