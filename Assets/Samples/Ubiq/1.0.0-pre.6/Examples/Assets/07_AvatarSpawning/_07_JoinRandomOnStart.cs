using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Rooms;

namespace Ubiq.Examples
{
    public class _07_JoinRandomOnStart : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<RoomClient>().Join(Guid.NewGuid());
        }
    }
}