using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class CursorManager : MonoBehaviour
    {
        public Texture2D texture;

        void Start()
        {
            Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
        }
    }
}
