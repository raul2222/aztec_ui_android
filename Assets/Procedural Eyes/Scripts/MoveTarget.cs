using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    public float speed = 10.0f; // Puedes ajustar la velocidad en el Inspector de Unity
    private TcpListener _listener;
    private const int _port = 8081;
    Vector3 move = new Vector3(0f, 0f, -20f);

    private void Start()
    {
        _listener = new TcpListener(IPAddress.Any, _port);
        _listener.Start();
        _listener.BeginAcceptTcpClient(AcceptTcpClient, _listener);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        var listener = (TcpListener) ar.AsyncState;
        var client = listener.EndAcceptTcpClient(ar);
        _listener.BeginAcceptTcpClient(AcceptTcpClient, _listener);

        var bytes = new byte[1024];
        var stream = client.GetStream();

        while (client.Connected)
        {
            var length = stream.Read(bytes, 0, bytes.Length);
            if (length == 0) break;

            var data = Encoding.UTF8.GetString(bytes, 0, length);
            Debug.Log("Received: " + data);
            if (data.Contains("left")){
                left();
            }
            if (data.Contains("right")){
                right();
            }
            if (data.Contains("normal")){
                normal();
            }

            var response = Encoding.UTF8.GetBytes("Hello from Unity server");
            stream.Write(response, 0, response.Length);
        }

        client.Close();
    }

    private void OnApplicationQuit()
    {
        _listener.Stop();
    }

    void run(){

    }

    private void left(){

        move = new Vector3(-1.91f, 0.0f,-20.0f);

    }

    private void normal(){

        move = new Vector3(0f, 0.0f,-20.0f);

    }

    private void right(){

        move = new Vector3(1.91f, 0.0f,-20.0f);

    }

    // Update se llama una vez por fotograma
    void Update()
    {
        
        transform.position = move ;
        //Debug.Log (transform.position);
    }
}
