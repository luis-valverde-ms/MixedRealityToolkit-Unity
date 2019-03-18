using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Input2
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager instance = null;
        public static InputManager Instance { get { return instance; } }

        private List<Controller> controllers = new List<Controller>();

        public T GetOrCreateController<T>() where T : Controller, new()
        {
            foreach (var c in controllers)
            {
                var typedController = c as T;
                if (c != null)
                {
                    return typedController;
                }
            }

            var controller = new T();
            controllers.Add(controller);
            return controller;
        }

        private void Awake()
        {
            instance = this;
        }

        void Update()
        {
            foreach (var c in controllers)
            {
                c.Tick();
            }
        }
    }
}