using System.Collections.Generic;
using UnityEngine;

namespace IdleCarService.Build
{
    public enum BuildingZoneType
    {
        None,
        Warehouse,
        Station,
        Factory
    }
    
    [RequireComponent(typeof(LineRenderer))]
    public class BuildingZone : MonoBehaviour
    {
        public BuildingZoneType ZoneType => _zoneType;
        public int AvailableSlots => _buildingSlots.Count - _placedBuildings.Count;

        [SerializeField] private BuildingZoneType _zoneType;
        [SerializeField] private float _buildingLength;
        [SerializeField] private float _buildingWidth;
        [SerializeField] private List<Transform> _buildingSlots = new List<Transform>();
        [SerializeField] private LineRenderer _zoneOutline;
        [SerializeField] private Color _zoneColor = Color.cyan;
        [SerializeField] private float _outlineWidth = 0.1f;
        [SerializeField] private float _outlineHeight = 0.05f;

        private List<Building> _placedBuildings = new List<Building>();

        private void OnValidate()
        {
            if (_zoneOutline == null)
            {
                if (TryGetComponent(out _zoneOutline) == false)
                    _zoneOutline = gameObject.AddComponent<LineRenderer>();
                
                _zoneOutline.startWidth = _outlineWidth;
                _zoneOutline.endWidth = _outlineWidth;
                _zoneOutline.material = new Material(Shader.Find("Sprites/Default"));
            }
        }

        public void Init()
        {
            InitializeZoneOutline();
        }

        private void InitializeZoneOutline()
        {
            _zoneOutline.startColor = _zoneColor;
            _zoneOutline.endColor = _zoneColor;

            UpdateZoneOutline();
            _zoneOutline.enabled = true;
        }

        private void UpdateZoneOutline()
        {
            if (_buildingSlots.Count == 0)
                return;

            Vector3 center = Vector3.zero;

            foreach (Transform slot in _buildingSlots)
                center += slot.position;

            center /= _buildingSlots.Count;

            float xOffset = _buildingWidth / 2f;
            float zOffset = (_buildingLength / 2f) * _buildingSlots.Count;

            Vector3[] outlinePoints = new Vector3[5];
            outlinePoints[0] = new Vector3(center.x - xOffset, _outlineHeight, center.z - zOffset);
            outlinePoints[1] = new Vector3(center.x + xOffset, _outlineHeight, center.z - zOffset);
            outlinePoints[2] = new Vector3(center.x + xOffset, _outlineHeight, center.z + zOffset);
            outlinePoints[3] = new Vector3(center.x - xOffset, _outlineHeight, center.z + zOffset);
            outlinePoints[4] = outlinePoints[0];

            _zoneOutline.positionCount = outlinePoints.Length;
            _zoneOutline.SetPositions(outlinePoints);
        }

        public bool CanPlaceBuilding(out Transform freeSlot)
        {
            freeSlot = null;

            if (AvailableSlots <= 0)
                return false;

            foreach (Transform slot in _buildingSlots)
            {
                if (IsSlotOccupied(slot) == false)
                {
                    freeSlot = slot;
                    return true;
                }
            }

            return false;
        }

        public void AddBuilding(Building building, Transform freeSlot)
        {
            if (building == null || freeSlot == null)
                return;

            foreach (Transform slot in _buildingSlots)
            {
                if (slot == freeSlot)
                {
                    building.transform.SetParent(freeSlot);
                    _placedBuildings.Add(building);
                }
            }
        }

        private bool IsSlotOccupied(Transform slot)
        {
            foreach (Building building in _placedBuildings)
            {
                if (Vector3.Distance(building.transform.position, slot.position) < 0.1f)
                    return true;
            }

            return false;
        }

        public void ExpandZone(Vector3 direction, int slotsToAdd = 1)
        {
            Vector3 expansionDirection = direction.normalized;

            Transform furthestSlot = _buildingSlots[0];
            float furthestDistance = Vector3.Dot(furthestSlot.position, expansionDirection);

            foreach (Transform slot in _buildingSlots)
            {
                float distance = Vector3.Dot(slot.position, expansionDirection);
                if (distance > furthestDistance)
                {
                    furthestDistance = distance;
                    furthestSlot = slot;
                }
            }

            for (int i = 0; i < slotsToAdd; i++)
            {
                GameObject newSlotObj = new GameObject($"BuildingSlot_{_buildingSlots.Count + 1}");
                newSlotObj.transform.parent = transform;
                newSlotObj.transform.position = furthestSlot.position + expansionDirection * _buildingLength * (i + 1);

                _buildingSlots.Add(newSlotObj.transform);
            }

            UpdateZoneOutline();
        }

        public void RebuildZone(Vector3 direction, int targetSlotCount)
        {
            int currentSlotCount = _buildingSlots.Count;
            
            if (targetSlotCount > currentSlotCount)
            {
                int slotsToAdd = targetSlotCount - currentSlotCount;
                ExpandZone(direction, slotsToAdd);
            }
            else if (targetSlotCount < currentSlotCount)
            {
                int slotsToRemove = currentSlotCount - targetSlotCount;
                RemoveSlots(slotsToRemove);
            }

            UpdateZoneOutline();
        }
        
        private void RemoveSlots(int slotsToRemove)
        {
            for (int i = 0; i < slotsToRemove; i++)
            {
                Transform slotToRemove = _buildingSlots[^1];
                _buildingSlots.RemoveAt(_buildingSlots.Count - 1);
                Destroy(slotToRemove.gameObject);
            }
        }
    }
}