using System.Collections.Generic;
using UnityEngine;

namespace IdleCarService.Unit
{
    public class ClientManager : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _targetPoint;

        private ClientsConfig _config;

        private int _activeClients = 0;
        private bool _isSpawning = false;
        private Queue<Client> _clientPool = new Queue<Client>();

        public void Init(ClientsConfig config)
        {
            _config = config;
            
            CreatePool(_config.StartPool);
        }
        
        public void ToggleSpawning(bool isSpawning)
        {
            _isSpawning = isSpawning;
            
            if (_isSpawning && _activeClients < _config.MaxClientsOnRoad)
                StartCoroutine(SpawnClientsRoutine());
            else
                StopAllCoroutines();
        }

        private System.Collections.IEnumerator SpawnClientsRoutine()
        {
            while (_isSpawning)
            {
                if (_activeClients < _config.MaxClientsOnRoad)
                {
                    SpawnClient();
                    _activeClients++;
                }

                yield return new WaitForSeconds(_config.SpawnInterval);
            }
        }

        private void SpawnClient()
        {
            Client client;
            
            if (_clientPool.Count > 0)
            {
                client = _clientPool.Dequeue();
                client.gameObject.SetActive(true);
            }
            else
            {
                client = Instantiate(_config.Prefabs[Random.Range(0, _config.Prefabs.Length)], _spawnPoint);
                client.Init(Random.Range(_config.MinSpeed, _config.MaxSpeed), _config.RotateSpeed);
            }

            client.transform.position = _spawnPoint.position;
            client.transform.rotation = _spawnPoint.rotation;
            client.InitForRoad(_targetPoint, OnClientFinished);
        }
        
        private void CreatePool(int poolSize)
        {
            for (int i = 0; i < _config.Prefabs.Length; i++)
            {
                Client client = Instantiate(_config.Prefabs[i]);
                client.gameObject.SetActive(false);
                client.Init(Random.Range(_config.MinSpeed, _config.MaxSpeed), _config.RotateSpeed);
                _clientPool.Enqueue(client);
            }

            if (_clientPool.Count < poolSize)
                CreatePool(poolSize - _clientPool.Count);
        }

        private void OnClientFinished(Client client)
        {
            _activeClients--;
            
            client.StopMoving();
            client.gameObject.SetActive(false);
            _clientPool.Enqueue(client);
        }
    }
}