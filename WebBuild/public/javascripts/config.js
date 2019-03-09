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
  "devP": {
    "hipr_restful": "http://localhost:8086/api/1.0",
    "eth": "main"
  },
  "production1": {
    "hipr_restful": "http://ec2-18-219-170-189.us-east-2.compute.amazonaws.com:8086/api/1.0",
    "eth": "main"
  },
  "productionH": {
    "hipr_restful": "https://hipr.one:8086/api/1.0",
    "eth": "main"
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
              1011
            ]
          },
          "compiler": {
            "name": "solc",
            "version": "0.4.24+commit.e67f0147.Emscripten.clang"
          },
          "updatedAt": "2019-03-09T19:10:27.181Z",
          "validation": {
            "hash": "491a2b12f4c676d33ae5b058c4eb91b136f142cf9daedf2292032a416a629ef1",
            "deployDate": "2019-03-09T19:10:28.176Z",
            "deployBy": "HERC Team",
            "abiHash": "da93a7c2ca07139bf30a86a007e8b278f8b49fbce1d4b3ada773e3850db795fc",
            "sourceSize": 28216,
            "sourceLines": 336,
            "sourceHash": "705481fd5cd8a2647fd211a9ddc7afc44fb091d2a52460ab1a2c1701ff9d0cd7",
            "bytecodeHash": "839176fea52d6bcf2bf40e804a5042b95093eac62cb5122e1eafc6539e52d0f7",
            "buildHash": "3e89245ecc40b1c25ed3fc3f56279aa360f578f4195d88b26a9129370d17ae68"
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
              483
            ]
          },
          "compiler": {
            "name": "solc",
            "version": "0.4.24+commit.e67f0147.Emscripten.clang"
          },
          "updatedAt": "2019-03-09T19:10:27.168Z",
          "validation": {
            "hash": "b59d1e18679eba6c980ac063b64d38a1ed35d4dbfdc1dbda5833e14d04755079",
            "deployDate": "2019-03-09T19:10:29.052Z",
            "deployBy": "HERC Team",
            "abiHash": "66d04ced002f87c1aad27660584cf20bf8f273af02a92a17f0dabe13367480e8",
            "sourceSize": 8101,
            "sourceLines": 140,
            "sourceHash": "ec3035e566c6a987b893157a45b221d55cb0821f3da72c861581b7fa22fa0988",
            "bytecodeHash": "c7a3e00d697ef00f44400e0a255defce86fdd68d77b53a93835a914aff0de91c",
            "buildHash": "2758c1b11ce408a00861f3d7091e217b89fd19eff688294f3f8e8233a600ffc1"
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
    },
    "main": {
      "PlayerScore": {
        "options": {},
        "address": "0x0e3025cd91635cb6221fae180cbf7097f33d1817",
        "validation": {
          "contractName": "PlayerScore",
          "sourcePath": "",
          "exportedSymbols": {
            "PlayerScore": [
              991
            ]
          },
          "compiler": {
            "name": "solc",
            "version": "0.4.24+commit.e67f0147.Emscripten.clang"
          },
          "updatedAt": "2019-03-05T11:09:40.601Z",
          "validation": {
            "hash": "57a70a61353704c9520d005d789aec1b5a17432b0b19b423050fe67053e44246",
            "deployDate": "2019-03-05T11:09:41.614Z",
            "deployBy": "HERC Team",
            "abiHash": "c45811ba44eeda7d6d81816425d6ae5f1a2a31cf724629cb665d81064f50cfc2",
            "sourceSize": 27816,
            "sourceLines": 334,
            "sourceHash": "bdf301f7ce548ed0485008d663d9cf980e981c4207397ae440c574bb6ab0938b",
            "bytecodeHash": "8d5e592a9d2334a8254d3e3984217eb17e5bf3b2c62025efe48124648446dd2a",
            "buildHash": "f692f1d03ace4258eb2dbbb9590c72e46607efbbc93af154fb8a30ef88120319"
          }
        }
      },
      "PuzzleManager": {
        "options": {},
        "address": "0x28252ef7ae8079a0f1fa5c31522bc2c735b6ac9c",
        "validation": {
          "contractName": "PuzzleManager",
          "sourcePath": "",
          "exportedSymbols": {
            "PuzzleManager": [
              483
            ]
          },
          "compiler": {
            "name": "solc",
            "version": "0.4.24+commit.e67f0147.Emscripten.clang"
          },
          "updatedAt": "2019-03-05T11:09:40.589Z",
          "validation": {
            "hash": "c89e72eb9ce3248425d1deac8cba0355360983ce6a96c39fd133b4d64098ed94",
            "deployDate": "2019-03-05T11:09:42.468Z",
            "deployBy": "HERC Team",
            "abiHash": "66d04ced002f87c1aad27660584cf20bf8f273af02a92a17f0dabe13367480e8",
            "sourceSize": 8101,
            "sourceLines": 140,
            "sourceHash": "ec3035e566c6a987b893157a45b221d55cb0821f3da72c861581b7fa22fa0988",
            "bytecodeHash": "c7a3e00d697ef00f44400e0a255defce86fdd68d77b53a93835a914aff0de91c",
            "buildHash": "691b9ad932ac6feb183a23ea5aeac5053840242536696cc2e513770f7a315a73"
          }
        }
      },
      "HERCToken": {
        "options": {},
        "address": "0xfa8353be80079d6e29aec5432606fd5041d6a1e7",
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
          "updatedAt": "2019-01-03T21:15:11.486Z",
          "validation": {
            "hash": "45cd2d1a94f6c0978aaa7ebbf74f7727e7eab00e46dedbba55416e4e481aac41",
            "deployDate": "2019-01-03T21:15:12.431Z",
            "deployBy": "HERC Team",
            "abiHash": "d2758fee2c4839ce939d1b00c20e7acea61940b64170c4a4ad2ace1a27ffe9ea",
            "sourceSize": 11958,
            "sourceLines": 153,
            "sourceHash": "028883e390f1b17b50a98077d0934ea42b020bbb0ce2586e0f77b65425b84619",
            "bytecodeHash": "5435dfd5afb1cd6fec3b8325a6ea41afe608c061b464ab018b2721afc20f2d0b",
            "buildHash": "8249b54cf109e0f318e68bacbcbc89f0155ec9a6d8b14b944916d112ad1663a0"
          }
        }
      }
    }
  }
}