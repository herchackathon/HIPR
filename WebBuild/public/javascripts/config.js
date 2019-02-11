Web3Options = {
  "env": "dev",
  "dev": {
    "hipr_restful": "http://localhost:8086/api/1.0",
    "eth": "ganache"
  },
  "devR": {
    "hipr_restful": "http://localhost:8086/api/1.0",
    "eth": "ropsten"
  },
  "production": {
    "hipr_restful": "http://amazon:8086/api/1.0",
    "eth": "mainnet"
  },
  "contracts": {
    "mainnet": {
      "PlayerScore": {
        "address": "0xf1886887f07ef5392c41a976cec1eb8443dcad85",
        "validation": {
          "hash": "",
          "deployDate": "",
          "deployBy": "",
          "abiHash": "",
          "sourceSize": 0,
          "sourceLines": 0,
          "sourceHash": ""
        }
      },
      "PuzzleManager": {
        "address": "0xf792c43f23c39f7de185cfdc6ce96aa69e9f00c1",
        "validation": {
          "hash": "",
          "deployDate": "",
          "deployBy": "",
          "abiHash": "",
          "sourceSize": 0,
          "sourceLines": 0,
          "sourceHash": ""
        }
      }
    },
    "ropsten": {
      "PlayerScore": {
        "address": "0x11ee9ff611bd9d24fd760aef7980e0dd977f9f63",
        "validation": {
          "contractName": "PlayerScore",
          "sourcePath": "",
          "exportedSymbols": {
            "PlayerScore": [
              469
            ]
          },
          "compiler": {
            "name": "solc",
            "version": "0.4.24+commit.e67f0147.Emscripten.clang"
          },
          "updatedAt": "2019-02-11T12:35:06.891Z",
          "validation": {
            "hash": "9370c5f2220f84358c3b5953dd63b5e8cda6a6e70e9015dd5a582ec982792a23",
            "deployDate": "2019-02-11T12:35:07.946Z",
            "deployBy": "HERC Team",
            "abiHash": "754bb42e7e4e0a457964a45845c43387dba3ae0cfb653b1036d31c8f93a70c1c",
            "sourceSize": 18068,
            "sourceLines": 218,
            "sourceHash": "3113d83969bba410a8348d89fd0c1678f5a63b1379922e06158226d0ea4cd67f",
            "bytecodeHash": "662e130967d3b596cc51464cd5031c2ef2764294711327b18f72b075b95d4368",
            "buildHash": "13b7f1b5a382598854f61200ed9e62d7e6559041eff8a2145876a5ab27527324"
          }
        },
        "options": {}
      },
      "PuzzleManager": {
        "address": "0xf0831e19dadc09d13b1189a8c8b39b5ed90c23ac",
        "validation": {
          "contractName": "PuzzleManager",
          "sourcePath": "",
          "exportedSymbols": {
            "PuzzleManager": [
              464
            ]
          },
          "compiler": {
            "name": "solc",
            "version": "0.4.24+commit.e67f0147.Emscripten.clang"
          },
          "updatedAt": "2019-02-11T12:35:06.884Z",
          "validation": {
            "hash": "72474c1fddd2b29804e654702f0ef83886f2cb63e76c5dcd06a7247f719de4ae",
            "deployDate": "2019-02-11T12:35:08.901Z",
            "deployBy": "HERC Team",
            "abiHash": "adbc4184e514c05b0519c1736e80fa0dfa7cde1a22e399f1bd778be7805634f1",
            "sourceSize": 7920,
            "sourceLines": 137,
            "sourceHash": "eaf972332db6e14fd935bd5ef508058d76dc6bcd0b11e1928f2f6fd915698ee1",
            "bytecodeHash": "2738f4d1f37479f52003ad3bb53e20240b6d165588ef7c5b443ef14357636324",
            "buildHash": "b33abc1ff9e9dc795b1b8ee9bf3b7db90a5d6f2d9d405a1424bcf58d58f638bc"
          }
        },
        "options": {}
      },
      "HERCToken": {
        "options": {},
        "address": "0xf481e3694434a3f7e33d2545205d4dea7ba2c94b",
        "validation": {
          "contractName": "HERCToken",
          "sourcePath": "",
          "exportedSymbols": {
            "HERCToken": [
              866
            ],
            "Int20": [
              251
            ],
            "Ownable": [
              358
            ],
            "SafeMath": [
              184
            ]
          },
          "compiler": {
            "name": "solc",
            "version": "0.4.24+commit.e67f0147.Emscripten.clang"
          },
          "updatedAt": "2019-01-31T19:25:44.298Z",
          "validation": {
            "hash": "31d179a34c62677652cc91a18c8e5e97109b1d0969b2835771ecf2ccb227c44b",
            "deployDate": "2019-01-31T19:25:45.211Z",
            "deployBy": "HERC Team",
            "abiHash": "d2758fee2c4839ce939d1b00c20e7acea61940b64170c4a4ad2ace1a27ffe9ea",
            "sourceSize": 11958,
            "sourceLines": 153,
            "sourceHash": "028883e390f1b17b50a98077d0934ea42b020bbb0ce2586e0f77b65425b84619",
            "bytecodeHash": "5435dfd5afb1cd6fec3b8325a6ea41afe608c061b464ab018b2721afc20f2d0b",
            "buildHash": "f3333df059d0b25e5d507df8fdea502d509fdce0ae7748d3f2d75102eac5acdc"
          }
        }
      }
    },
    "ganache": {
      "PlayerScore": {
        "options": {},
        "address": "0x11ee9ff611bd9d24fd760aef7980e0dd977f9f63",
        "validation": {
          "contractName": "PlayerScore",
          "sourcePath": "",
          "exportedSymbols": {
            "PlayerScore": [
              469
            ]
          },
          "compiler": {
            "name": "solc",
            "version": "0.4.24+commit.e67f0147.Emscripten.clang"
          },
          "updatedAt": "2019-02-11T12:35:06.891Z",
          "validation": {
            "hash": "9370c5f2220f84358c3b5953dd63b5e8cda6a6e70e9015dd5a582ec982792a23",
            "deployDate": "2019-02-11T12:35:07.946Z",
            "deployBy": "HERC Team",
            "abiHash": "754bb42e7e4e0a457964a45845c43387dba3ae0cfb653b1036d31c8f93a70c1c",
            "sourceSize": 18068,
            "sourceLines": 218,
            "sourceHash": "3113d83969bba410a8348d89fd0c1678f5a63b1379922e06158226d0ea4cd67f",
            "bytecodeHash": "662e130967d3b596cc51464cd5031c2ef2764294711327b18f72b075b95d4368",
            "buildHash": "13b7f1b5a382598854f61200ed9e62d7e6559041eff8a2145876a5ab27527324"
          }
        }
      },
      "PuzzleManager": {
        "options": {},
        "address": "0xf0831e19dadc09d13b1189a8c8b39b5ed90c23ac",
        "validation": {
          "contractName": "PuzzleManager",
          "sourcePath": "",
          "exportedSymbols": {
            "PuzzleManager": [
              464
            ]
          },
          "compiler": {
            "name": "solc",
            "version": "0.4.24+commit.e67f0147.Emscripten.clang"
          },
          "updatedAt": "2019-02-11T12:35:06.884Z",
          "validation": {
            "hash": "72474c1fddd2b29804e654702f0ef83886f2cb63e76c5dcd06a7247f719de4ae",
            "deployDate": "2019-02-11T12:35:08.901Z",
            "deployBy": "HERC Team",
            "abiHash": "adbc4184e514c05b0519c1736e80fa0dfa7cde1a22e399f1bd778be7805634f1",
            "sourceSize": 7920,
            "sourceLines": 137,
            "sourceHash": "eaf972332db6e14fd935bd5ef508058d76dc6bcd0b11e1928f2f6fd915698ee1",
            "bytecodeHash": "2738f4d1f37479f52003ad3bb53e20240b6d165588ef7c5b443ef14357636324",
            "buildHash": "b33abc1ff9e9dc795b1b8ee9bf3b7db90a5d6f2d9d405a1424bcf58d58f638bc"
          }
        }
      },
      "HERCToken": {
        "options": {},
        "address": "0xf481e3694434a3f7e33d2545205d4dea7ba2c94b",
        "validation": {
          "contractName": "HERCToken",
          "sourcePath": "",
          "exportedSymbols": {
            "HERCToken": [
              866
            ],
            "Int20": [
              251
            ],
            "Ownable": [
              358
            ],
            "SafeMath": [
              184
            ]
          },
          "compiler": {
            "name": "solc",
            "version": "0.4.24+commit.e67f0147.Emscripten.clang"
          },
          "updatedAt": "2019-01-31T19:25:44.298Z",
          "validation": {
            "hash": "31d179a34c62677652cc91a18c8e5e97109b1d0969b2835771ecf2ccb227c44b",
            "deployDate": "2019-01-31T19:25:45.211Z",
            "deployBy": "HERC Team",
            "abiHash": "d2758fee2c4839ce939d1b00c20e7acea61940b64170c4a4ad2ace1a27ffe9ea",
            "sourceSize": 11958,
            "sourceLines": 153,
            "sourceHash": "028883e390f1b17b50a98077d0934ea42b020bbb0ce2586e0f77b65425b84619",
            "bytecodeHash": "5435dfd5afb1cd6fec3b8325a6ea41afe608c061b464ab018b2721afc20f2d0b",
            "buildHash": "f3333df059d0b25e5d507df8fdea502d509fdce0ae7748d3f2d75102eac5acdc"
          }
        }
      }
    }
  }
}